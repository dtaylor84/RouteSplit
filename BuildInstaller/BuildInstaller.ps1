param([switch]$TestMode)
$ErrorActionPreference = "Stop"
###############################################################################
# build.ps1
#
# Automatically update BuildConstants and .wxs, then rebuild .dll and .msi
# Generates GUIDs for new ProgIdVersions as required
#
# -TestMode The installer is not saved, but is installed immediately.
#           New GUIDs are generated as required, but not saved either.
#
# David Taylor
# 2013-10-27
###############################################################################
if ($TestMode) {
  $MsBuildConfiguration = "Debug";
} else {
  $MsBuildConfiguration = "Release";
}
####################
# File Locations
####################
$ShortName="RouteSplit"
$SolutionDir = "U:\My Documents\Visual Studio 2010\Projects\$ShortName"
$Solution = "$SolutionDir\$ShortName.sln"
$ProjectDir = "$SolutionDir\$ShortName"
$InstallerDir = "$SolutionDir\BuildInstaller"
$MSIDir = "$SolutionDir\MSI"
$Assembly = "$SolutionDir\$MsBuildConfiguration\$ShortName.dll"
# GUID database - updated with new GUIDs
$ReleaseDB = "$InstallerDir\BuildInstaller-DB.xml"
# Outputs - generated c# constants / wix definitions
$BuildConstants = "$InstallerDir\BuildInstaller-consts.cs"
$WixInclude = "$InstallerDir\BuildInstaller-defs.wxi"
$WixToolsetDir = "$env:WIX"
####################
# Project Properties
####################
. "$InstallerDir\BuildInstaller-properties.ps1"
$ProductName="$ShortName SAP GUI Control"
$Manufacturer="Menzies Distribution Ltd"
$Copyright="Copyright © $Manufacturer 2013"
$ControlName="${ShortName}Ctrl" # Control Name
$InterfaceMain="${ControlName}Intf" # COM Interface name
$InterfaceEvents="${ControlName}Events" # COM Disp Interface name
$InterfaceMainProxy="00020424-0000-0000-C000-000000000046" # Universal Marshaller
$InterfaceEventsProxy="00020424-0000-0000-C000-000000000046"  # Universal Marshaller
$RuntimeVersion="v4.0.30319" # Target .NET framework version
$Language="2057"
$PublicKeyToken="e07ad4a9cb89687a"
$ProgId = "$ShortName.$ControlName"
#########################
# Installation Properties
#########################
## NEVER CHANGE InstallDir or UpgradeCode -- it will break upgrade/uninstall
$InstallDir="$ShortName" # Install location (within ProgramFilesFolder)
$UpgradeCode="E3B2267A-77D4-4D28-9321-5140EB89F517" # Links all releases together
###############################################################################
# Load Visual Studio environment variables, for running MSBuild.exe later
if ($env:VSINSTALLDIR -eq $null) {
	pushd "$env:ProgramFiles\Microsoft Visual Studio 10.0\VC"
	cmd /c "vcvarsall.bat && set" |
		foreach {
			if ($_ -match "=") {
				$v = $_.split("="); set-item -force -path "ENV:\$($v[0])"  -value "$($v[1])"
			}
		}
	if ($LastExitCode -ne 0) { throw "Failed to set up VS environment" }
	popd
}
###############################################################################

$TimeSinceEpoch = New-TimeSpan $(Get-Date -month 1 -day 1 -year 2000 -Hour 0 -Minute 0 -Second 0) $(Get-Date)
# Calculate Build number (whole days since 2000-01-01)
$BuildNumber = $TimeSinceEpoch.Days
# Calculate Revision number (2-second intervals since midnight)
$RevisionNumber = [int] (($TimeSinceEpoch.Hours * 3600 + $TimeSinceEpoch.Minutes * 60 + $TimeSinceEpoch.Seconds)/2)
# Construct version numbers
$MajorVersion = 1
$FileVersion = $MajorVersion.ToString() + "." + $ProgIdVersion.ToString() + "." + $BuildNumber.ToString() + "." + $RevisionNumber.ToString()
$AssemblyVersion = $MajorVersion.ToString() + "." + $ProgIdVersion.ToString() + ".0.0"
$NextAssemblyVersion = $MajorVersion.ToString() + "." + ($ProgIdVersion + 1).ToString() + ".0.0"
$RetainVersion = $MajorVersion.ToString() + "." + $RetainProgIdVersion.ToString() + ".0.0" 
$ProgIdVersioned = "$ProgId." + $ProgIdVersion.ToString()

# read existing release database
$xml = New-Object -TypeName XML
$xml.Load($ReleaseDB)

# check for existing guids for this $ProgIdVersion
$XmlVersionSelect = Select-Xml -Xml $xml -XPath "/ReleaseDB/Versions/Version[@ProgIdVersion='$ProgIdVersion']"


# prompt for confirmation
$wshell = New-Object -ComObject Wscript.Shell -ErrorAction Stop
if ($TestMode)
{
	$RetainProgIdVersion=1 # retain all versions for TestMode builds
	$rc = $wshell.Popup(("Compile and install new v{0} debug build?" -f $ProgIdVersion),0,"Debug Build",32+1)	
}
elseif ($XmlVersionSelect -eq $null)
{
	$rc = $wshell.Popup(("Generate guids and create new v{0} release? (keep >=v{1})" -f $ProgIdVersion, $RetainProgIdVersion),0,"Release Build",32+1)
}
else
{
	$rc = $wshell.Popup(("Re-roll existing v{0} release using existing GUIDs? (keep >=v{1}) (Only do this if v{0} has never been deployed!)" -f $ProgIdVersion, $RetainProgIdVersion),0,"Replace Release",48+1)
}

if ($rc -ne 1) {
  exit
}

if ($XmlVersionSelect -eq $null)
{
	# generate <Version> with new guids and append it to the <Versions> element
	$XmlVersion = $xml.CreateElement('Version')
	[Void]$XmlVersion.SetAttribute('ProgIdVersion', $ProgIdVersion)
	$XmlVersion.AppendChild($xml.CreateElement('VersionedProgId')).InnerText = $ProgIdVersioned
	$XmlVersion.AppendChild($xml.CreateElement('Clsid')).InnerText = [guid]::NewGuid().ToString("D").ToUpper()
	$XmlVersion.AppendChild($xml.CreateElement('TypeLibGuid')).InnerText = [guid]::NewGuid().ToString("D").ToUpper()
	$XmlVersion.AppendChild($xml.CreateElement('InterfaceMainGuid')).InnerText = [guid]::NewGuid().ToString("D").ToUpper()
	$XmlVersion.AppendChild($xml.CreateElement('InterfaceEventsGuid')).InnerText = [guid]::NewGuid().ToString("D").ToUpper()
	[Void]$xml.ReleaseDB['Versions'].AppendChild($XmlVersion) 
}
else
{
  $XmlVersion = $XmlVersionSelect.Node
}

# Generate <Build> with new version numbers and append it to the <Builds> element
$XmlBuild = $xml.CreateElement('Build')
[Void]$XmlBuild.SetAttribute('Version', $FileVersion)
$XmlBuild.AppendChild($xml.CreateElement('ProductId')).InnerText = [guid]::NewGuid().ToString("D").ToUpper()
$XmlBuild.AppendChild($xml.CreateElement('AssemblyVersion')).InnerText = $AssemblyVersion
$XmlBuild.AppendChild($xml.CreateElement('RetainVersion')).InnerText = $RetainVersion
$XmlBuild.AppendChild($xml.CreateElement('Timestamp')).InnerText = $(Get-Date -Format 's').ToString()
[Void]$xml.ReleaseDB['Builds'].AppendChild($XmlBuild)

# generate buildconstants file
@"
/* DO NOT EDIT - Auto Generated by build.ps1! */
namespace $ShortName
{
    static class BuildConstants
    {
        public const string Guid_TypeLib = "$($XmlVersion.TypeLibGuid)";
        public const string Guid_$ControlName = "$($XmlVersion.Clsid)";
        public const string Guid_$InterfaceMain = "$($XmlVersion.InterfaceMainGuid)";
        public const string Guid_$InterfaceEvents = "$($XmlVersion.InterfaceEventsGuid)";

        public const string ProgId = "$ProgId";
        public const string ProgIdVersioned = "$ProgIdVersioned";
		
		public const int TypeLibVersionMajor = $ProgIdVersion;
		public const int TypeLibVersionMinor = 0;
		
		public const string MsBuildConfiguration = "$MsBuildConfiguration";

        public const string AssemblyVersion = "$AssemblyVersion";
        public const string AssemblyFileVersion = "$FileVersion";
        public const string AssemblyInformationalVersion = "$FileVersion";
        public const string AssemblyConfiguration = "";
        public const string AssemblyCulture = "";
        public const string AssemblyDescription = "$ProductName";
        public const string AssemblyProduct = "$ShortName";
        public const string AssemblyCopyright = "$Copyright";
        public const string AssemblyCompany = "$Manufacturer";
        public const string AssemblyTitle = "$ShortName";
        public const string AssemblyTrademark = "";
		$(
			foreach ($i in $DispIds.GetEnumerator()) {
			  "`r`n`t`tpublic const int DispId_{0} = {1};" -f $i.Name, $i.Value
			}
		)
    }
}
/* DO NOT EDIT - Auto Generated by build.ps1! */
"@ >$BuildConstants

# rebuild with new BuildConstants.cs file
msbuild /m "$Solution" /p:Configuration=$MsBuildConfiguration /p:Platform="Any CPU"
if ($LastExitCode -ne 0) { throw "Build failed" }

# check newly built assembly has expected version number
$NewAssemblyVersion = [System.Reflection.AssemblyName]::GetAssemblyName($Assembly).Version
$NewFileVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($Assembly).FileVersion
$NewProductVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($Assembly).ProductVersion

if ( $NewAssemblyVersion -ne $AssemblyVersion-or $FileVersion -ne $NewFileVersion -or $FileVersion -ne $NewProductVersion )
{
	Write-Host ("Expect {0}; got {1}" -f $AssemblyVersion, $NewAssemblyVersion)
	Write-Host ("Expect {0}; got {1}" -f $FileVersion, $NewFileVersion)
	Write-Host ("Expect {0}; got {1}" -f $FileVersion, $NewProductVersion)
	throw "Unexpected version"
}	

# generate wix definitions include file
@"
<!-- DO NOT MODIFY - AUTOMATICALLY GENERATED BY build.ps1 -->
<Include>
	<?define SolutionDir          = "$SolutionDir" ?>
    <?define ProgIdVersion        = "$ProgIdVersion" ?>
    <?define ProductId            = "{$($XmlBuild.ProductId)}" ?>
    <?define Clsid                = "{$($XmlVersion.Clsid)}" ?>
    <?define TypeLibGuid          = "{$($XmlVersion.TypeLibGuid)}" ?>
    <?define InterfaceMainGuid    = "{$($XmlVersion.InterfaceMainGuid)}" ?>
    <?define InterfaceEventsGuid  = "{$($XmlVersion.InterfaceEventsGuid)}" ?>
    <?define RuntimeVersion       = "$RuntimeVersion" ?>
    <?define AssemblyVersion      = "$AssemblyVersion" ?>
    <?define NextAssemblyVersion  = "$NextAssemblyVersion" ?>
    <?define ProductVersion       = "$FileVersion" ?>
    <?define RetainVersion        = "$RetainVersion" ?>
    <?define Language             = "$Language" ?>
    <?define PublicKeyToken       = "$PublicKeyToken" ?>
    <?define Manufacturer         = "$Manufacturer" ?>
    <?define ShortName            = "$ShortName" ?>
    <?define ProductName          = "$ProductName" ?>
    <?define ControlName          = "$ControlName" ?>
    <?define InterfaceMain        = "$InterfaceMain" ?>
    <?define InterfaceEvents      = "$InterfaceEvents" ?>
    <?define InterfaceMainProxy   = "{$InterfaceMainProxy}" ?>
    <?define InterfaceEventsProxy = "{$InterfaceEventsProxy}" ?>
    <?define InstallDir           = "$InstallDir" ?>
    <?define UpgradeCode = "{$UpgradeCode}" ?>
	<?define MsBuildConfiguration = "$MsBuildConfiguration" ?>
</Include>
<!-- DO NOT MODIFY - AUTOMATICALLY GENERATED BY build.ps1 -->
"@ > $WixInclude

[Void](mkdir -p "$InstallerDir\obj" -f)
# run wix
echo "Running wix...."
pushd "$WixToolsetDir"
bin\candle.exe -nologo "$InstallerDir\BuildInstaller.wxs" -out "$InstallerDir\obj\BuildInstaller.wixobj"
if ($LastExitCode -ne 0) { throw "Failed to run candle.exe" }
bin\light.exe  -nologo "$InstallerDir\obj\BuildInstaller.wixobj" -out "$InstallerDir\obj\$ShortName-$FileVersion.msi"
if ($LastExitCode -ne 0) { throw "Failed to run light.exe" }
popd

if ($TestMode)
{
    $wshell.Popup("Ensure SAP Logon test-instance is not running!",0,"Installing new build",48)
	Start-Process msiexec.exe -ArgumentList "/passive /i ""$InstallerDir\obj\$ShortName-$FileVersion.msi""" -Wait
	Remove-Item -Force "$InstallerDir\obj\$ShortName-$FileVersion.msi"
	Start-Process "Test In SAP.lnk"
	echo ("Successfully installed {0} v{1}" -f $ShortName, $FileVersion)
}
else
{
	# Save XML file
	$xml.Save($ReleaseDB + ".new")
	Move-Item ($ReleaseDB + ".new") $ReleaseDB -Force

	[Void](mkdir -p "$MSIDir" -f)
	Copy-Item "$InstallerDir\obj\$ShortName-$FileVersion.msi" "$MSIDir\$ShortName-$FileVersion.msi"
	

	echo ("Successfully built {0} v{1} in {2}" -f $ShortName, $FileVersion, "$MSIDir\$ShortName-$FileVersion.msi")
}

Start-Sleep 3