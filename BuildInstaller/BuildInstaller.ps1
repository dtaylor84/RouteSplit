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
####################
# File Locations
####################
$ShortName="RouteSplit"
$SolutionDir = "U:\Source\Repos\$ShortName"
$Solution = "$SolutionDir\$ShortName.sln"
$ProjectDir = "$SolutionDir\$ShortName"
$InstallerDir = "$SolutionDir\BuildInstaller"
$MSIDir = "$SolutionDir\MSI"
# GUID database - updated with new GUIDs
$ReleaseDB = "$InstallerDir\BuildInstaller-DB.xml"
# Outputs - generated c# constants / wix definitions
$BuildConstants = "$InstallerDir\BuildInstaller-consts.cs"
$WixInclude = "$InstallerDir\BuildInstaller-defs.wxi"
$WixToolsetDir = "$env:WIX"
####################
# Project Properties
####################
$BUILD_CONFIG = Import-CliXML "$InstallerDir\BuildInstaller-properties.xml"
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
	pushd "${env:ProgramFiles(x86)}\Microsoft Visual Studio 12.0\VC"
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
$wshell = New-Object -ComObject Wscript.Shell -ErrorAction Stop

function PromptUser([ref]$Config) {
			[void] [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
			[void] [System.Reflection.Assembly]::LoadWithPartialName("System.Drawing")  
			[System.Windows.Forms.Application]::EnableVisualStyles();
			$AnchorStyles = "System.Windows.Forms.AnchorStyles" -as [type] 
			$frmConfig = New-Object System.Windows.Forms.Form
			
            $frmConfignudVer =  New-Object System.Windows.Forms.NumericUpDown;
            $frmConfignudRetain = New-Object System.Windows.Forms.NumericUpDown;
            $frmConfiglblVer = New-Object System.Windows.Forms.Label;
            $frmConfiglblRetain = New-Object System.Windows.Forms.Label;
            $frmConfiglblEvents = New-Object System.Windows.Forms.Label;
            $frmConfigdgEvents = New-Object System.Windows.Forms.DataGridView;
            $frmConfiglblIntro = New-Object System.Windows.Forms.Label;
            $frmConfigbtnDebug = New-Object System.Windows.Forms.Button;
            $frmConfigbtnCancel = New-Object System.Windows.Forms.Button;
            $frmConfigEventName = New-Object System.Windows.Forms.DataGridViewTextBoxColumn;
            $frmConfigEventID = New-Object System.Windows.Forms.DataGridViewTextBoxColumn;
            $frmConfigbtnRelease = New-Object System.Windows.Forms.Button;
            ([System.ComponentModel.ISupportInitialize]($frmConfignudVer)).BeginInit();
            ([System.ComponentModel.ISupportInitialize]($frmConfignudRetain)).BeginInit();
            ([System.ComponentModel.ISupportInitialize]($frmConfigdgEvents)).BeginInit();
            $frmConfig.SuspendLayout();
            # 
            # lblIntro
            # 
            $frmConfiglblIntro.AutoSize = $True;
            $frmConfiglblIntro.Location = New-Object System.Drawing.Point(12, 9);
            $frmConfiglblIntro.Name = "lblIntro";
            $frmConfiglblIntro.Size = New-Object System.Drawing.Size(210, 13);
            $frmConfiglblIntro.TabIndex = 0;
            $frmConfiglblIntro.Text = "Configure properties and select build mode:";
            # 
            # lblVer
            # 
            $frmConfiglblVer.AutoSize = $True;
            $frmConfiglblVer.Location = New-Object System.Drawing.Point(12, 42);
            $frmConfiglblVer.Name = "lblVer";
            $frmConfiglblVer.Size = New-Object System.Drawing.Size(76, 13);
            $frmConfiglblVer.TabIndex = 1;
            $frmConfiglblVer.Text = "ProgId &Version:";
            # 
            # nudVer
            # 
            $frmConfignudVer.Location = New-Object System.Drawing.Point(274, 35);
			$frmConfignudVer.Anchor = ($AnchorStyles.Top -bor $AnchorStyles.Right)
            $frmConfignudVer.Maximum = 65534;
            $frmConfignudVer.Name = "nudVer";
            $frmConfignudVer.Size = New-Object System.Drawing.Size(75, 20);
            $frmConfignudVer.TabIndex = 2;
            # 
            # lblRetain
            # 
            $frmConfiglblRetain.AutoSize = $True;
            $frmConfiglblRetain.Location = New-Object System.Drawing.Point(12, 61);
            $frmConfiglblRetain.Name = "lblRetain";
            $frmConfiglblRetain.Size = New-Object System.Drawing.Size(251, 13);
            $frmConfiglblRetain.TabIndex = 3;
            $frmConfiglblRetain.Text = "Oldest ProgID Version to re&tain (Release only):";
            # 
            # nudRetain
            # 
            $frmConfignudRetain.Location = New-Object System.Drawing.Point(274, 61);
			$frmConfignudRetain.Anchor = ($AnchorStyles.Top -bor $AnchorStyles.Right)
            $frmConfignudRetain.Maximum = 65534;
            $frmConfignudRetain.Name = "nudRetain";
            $frmConfignudRetain.Size = New-Object System.Drawing.Size(75, 20);
            $frmConfignudRetain.TabIndex = 4;
            # 
            # lblEvents
            # 
            $frmConfiglblEvents.AutoSize = $True;
            $frmConfiglblEvents.Location = New-Object System.Drawing.Point(12, 112);
            $frmConfiglblEvents.Name = "lblEvents";
            $frmConfiglblEvents.Size = New-Object System.Drawing.Size(43, 13);
            $frmConfiglblEvents.TabIndex = 5;
            $frmConfiglblEvents.Text = "&Events:";
            # 
            # EventName
            # 
            $frmConfigEventName.HeaderText = "Name";
            $frmConfigEventName.MaxInputLength = 40;
            $frmConfigEventName.Name = "EventName";
            # 
            # EventID
            # 
            $frmConfigEventID.HeaderText = "ID";
            $frmConfigEventID.MaxInputLength = 9;
            $frmConfigEventID.Name = "EventID";	
            # 
            # dgEvents
            # 
            $frmConfigdgEvents.Anchor = ($AnchorStyles.Top -bor $AnchorStyles.Bottom -bor $AnchorStyles.Left -bor $AnchorStyles.Right)
            $frmConfigdgEvents.ColumnHeadersHeightSizeMode = [System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode]::AutoSize;
            $frmConfigdgEvents.Columns.AddRange($frmConfigEventName, $frmConfigEventID)
			$frmConfigdgEvents.SelectionMode = [System.Windows.Forms.DataGridViewSelectionMode]::FullRowSelect
            $frmConfigdgEvents.Location = New-Object System.Drawing.Point(61, 112);
            $frmConfigdgEvents.Name = "dgEvents";
            $frmConfigdgEvents.Size = New-Object System.Drawing.Size(288, 159);
            $frmConfigdgEvents.TabIndex = 6
            # 
            # btnDebug
            # 
            $frmConfigbtnDebug.Anchor = $AnchorStyles.Bottom -bor $AnchorStyles.Left;
            $frmConfigbtnDebug.Location = New-Object System.Drawing.Point(13, 290);
            $frmConfigbtnDebug.Name = "btnDebug";
            $frmConfigbtnDebug.Size = New-Object System.Drawing.Size(75, 23);
            $frmConfigbtnDebug.TabIndex = 7;
            $frmConfigbtnDebug.Text = "&Debug";
            $frmConfigbtnDebug.UseVisualStyleBackColor = $True;
            # 
            # btnRelease
            # 
            $frmConfigbtnRelease.Location = New-Object System.Drawing.Point(143, 290);
            $frmConfigbtnRelease.Name = "btnRelease";
            $frmConfigbtnRelease.Size = New-Object System.Drawing.Size(75, 23);
            $frmConfigbtnRelease.TabIndex = 8;
            $frmConfigbtnRelease.Text = "&RELEASE";
            $frmConfigbtnRelease.UseVisualStyleBackColor = $True;
            # 
            # btnCancel
            # 
            $frmConfigbtnCancel.Anchor = $AnchorStyles.Bottom -bor $AnchorStyles.Right;
            $frmConfigbtnCancel.Location = New-Object System.Drawing.Point(274, 290);
            $frmConfigbtnCancel.Name = "btnCancel";
            $frmConfigbtnCancel.Size = New-Object System.Drawing.Size(75, 23);
            $frmConfigbtnCancel.TabIndex = 9;
            $frmConfigbtnCancel.Text = "C&ancel";
            $frmConfigbtnCancel.UseVisualStyleBackColor = $True;
            # 
            # frmConfig
            # 
            $frmConfig.AutoScaleDimensions = New-Object System.Drawing.SizeF(6, 13);
            $frmConfig.AutoScaleMode = [System.Windows.Forms.AutoScaleMode]::Font;
            $frmConfig.ClientSize = New-Object System.Drawing.Size(361, 325);
            $frmConfig.Controls.Add($frmConfiglblIntro);
            $frmConfig.Controls.Add($frmConfiglblVer);
            $frmConfig.Controls.Add($frmConfignudVer);
            $frmConfig.Controls.Add($frmConfiglblRetain);
            $frmConfig.Controls.Add($frmConfignudRetain);
            $frmConfig.Controls.Add($frmConfiglblEvents);
            $frmConfig.Controls.Add($frmConfigdgEvents);
            $frmConfig.Controls.Add($frmConfigbtnDebug);
            $frmConfig.Controls.Add($frmConfigbtnRelease);
            $frmConfig.Controls.Add($frmConfigbtnCancel);
            $frmConfig.KeyPreview = $True;
            $frmConfig.MaximizeBox = $False;
            $frmConfig.MinimizeBox = $False;
            $frmConfig.Name = "frmConfig";
            $frmConfig.StartPosition = [System.Windows.Forms.FormStartPosition]::CenterScreen;
            $frmConfig.Text = "Build Configuration";
            $frmConfig.TopMost = $True;
            ([System.ComponentModel.ISupportInitialize]($frmConfignudVer)).EndInit();
            ([System.ComponentModel.ISupportInitialize]($frmConfignudRetain)).EndInit();
            ([System.ComponentModel.ISupportInitialize]($frmConfigdgEvents)).EndInit();
            $frmConfig.ResumeLayout( $False );
            $frmConfig.PerformLayout();

$DlgCancel = { $script:rc = 0; $frmConfig.Close(); }
$DlgDebug = { $script:rc = 1; $script:TestMode = $true; $frmConfig.Close() }
$DlgRelease = { $script:rc = 1; $script:TestMode = $false; $frmConfig.Close() }

$frmConfig.Add_KeyDown({ if ($_.KeyCode -eq "Escape") { $DlgCancel.Invoke() } })
$frmConfigbtnDebug.Add_Click($DlgDebug)
$frmConfigbtnRelease.Add_Click($DlgRelease)
$frmConfigbtnCancel.Add_Click($DlgCancel)
##############

	$frmConfignudVer.Value = $BUILD_CONFIG.ProgIdVersion
	$frmConfignudRetain.Value = $BUILD_CONFIG.RetainProgIdVersion

	foreach ($i in $BUILD_CONFIG.DispIds.GetEnumerator()) {
	  [void]$frmConfigdgEvents.Rows.Add( $i.Name, $i.Value)
	}

	[void] $frmConfig.ShowDialog()
	if ($rc -ne 1) { return $rc }

	# Should really validate this, but hey only programmers will use it and we never make mistakes.
	$BUILD_CONFIG.ProgIdVersion = $frmConfignudVer.Value
	$BUILD_CONFIG.RetainProgIdVersion = $frmConfignudRetain.Value

	$BUILD_CONFIG.DispIds = @{}
	foreach ($r in $frmConfigdgEvents.Rows) {
	  if ( $r.Cells['EventName'].Value) {
		$BUILD_CONFIG.DispIds[$r.Cells['EventName'].Value] = $r.Cells['EventID'].Value
	  }
	}		
	
	# prompt for confirmation
	if ($TestMode)
	{
		$rc = $wshell.Popup(("Compile and install new v{0} debug build?" -f $BUILD_CONFIG.ProgIdVersion),0,"Debug Build",32+1)	
	}
	elseif ($XmlVersionSelect -eq $null)
	{
		$rc = $wshell.Popup(("Generate guids and create new v{0} release? (keep >=v{1})" -f $BUILD_CONFIG.ProgIdVersion, $BUILD_CONFIG.RetainProgIdVersion),0,"Release Build",32+1)
	}
	else
	{
		$rc = $wshell.Popup(("Re-roll existing v{0} release using existing GUIDs? (keep >=v{1}) (Only do this if v{0} has never been deployed!)" -f $BUILD_CONFIG.ProgIdVersion, $BUILD_CONFIG.RetainProgIdVersion),0,"Replace Release",48+1)
	}

	return $rc
}

$rc = PromptUser ([ref]$BUILD_CONFIG)
if ($rc -ne 1) {
  exit
}

Export-CliXML -Path "$InstallerDir\BuildInstaller-properties.xml" -InputObject $BUILD_CONFIG

if ($TestMode) {
  $MsBuildConfiguration = "Debug";
} else {
  $MsBuildConfiguration = "Release";
}
$Assembly = "$SolutionDir\$MsBuildConfiguration\$ShortName.dll"

$TimeSinceEpoch = New-TimeSpan $(Get-Date -month 1 -day 1 -year 2000 -Hour 0 -Minute 0 -Second 0) $(Get-Date)
# Calculate Build number (whole days since 2000-01-01)
$BuildNumber = $TimeSinceEpoch.Days
# Calculate Revision number (2-second intervals since midnight)
$RevisionNumber = [int] (($TimeSinceEpoch.Hours * 3600 + $TimeSinceEpoch.Minutes * 60 + $TimeSinceEpoch.Seconds)/2)
# Construct version numbers
$MajorVersion = 1
$FileVersion = $MajorVersion.ToString() + "." + $BUILD_CONFIG.ProgIdVersion.ToString() + "." + $BuildNumber.ToString() + "." + $RevisionNumber.ToString()
$AssemblyVersion = $MajorVersion.ToString() + "." + $BUILD_CONFIG.ProgIdVersion.ToString() + ".0.0"
$NextAssemblyVersion = $MajorVersion.ToString() + "." + ($BUILD_CONFIG.ProgIdVersion + 1).ToString() + ".0.0"
	if ($TestMode)
	{
		$RetainVersion = $MajorVersion.ToString() + ".1.0.0" # retain all versions for TestMode builds
	} else {
		$RetainVersion = $MajorVersion.ToString() + "." + $BUILD_CONFIG.RetainProgIdVersion.ToString() + ".0.0" 
	}
$ProgIdVersioned = "$ProgId." + $BUILD_CONFIG.ProgIdVersion.ToString()

# read existing release database
$xml = New-Object -TypeName XML
$xml.Load($ReleaseDB)

# check for existing guids for this $BUILD_CONFIG.ProgIdVersion
$XmlVersionSelect = Select-Xml -Xml $xml -XPath "/ReleaseDB/Versions/Version[@BUILD_CONFIG.ProgIdVersion='$BUILD_CONFIG.ProgIdVersion']"


if ($XmlVersionSelect -eq $null)
{
	# generate <Version> with new guids and append it to the <Versions> element
	$XmlVersion = $xml.CreateElement('Version')
	[Void]$XmlVersion.SetAttribute('ProgIdVersion', $BUILD_CONFIG.ProgIdVersion)
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
		
		public const int TypeLibVersionMajor = $($BUILD_CONFIG.ProgIdVersion);
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
			foreach ($i in $BUILD_CONFIG.DispIds.GetEnumerator()) {
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
    <?define ProgIdVersion        = "$($BUILD_CONFIG.ProgIdVersion)" ?>
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
    <?define UpgradeCode          = "{$UpgradeCode}" ?>
	<?define MsBuildConfiguration = "$MsBuildConfiguration" ?>
</Include>
<!-- DO NOT MODIFY - AUTOMATICALLY GENERATED BY build.ps1 -->
"@ > $WixInclude

[Void](mkdir -Path "$InstallerDir\obj" -f)
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
    [void]$wshell.Popup("Ensure SAP Logon test-instance is not running!",0,"Installing new build",48)
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

	[Void](mkdir -Path "$MSIDir" -f)
	Copy-Item "$InstallerDir\obj\$ShortName-$FileVersion.msi" "$MSIDir\$ShortName-$FileVersion.msi"
	

	echo ("Successfully built {0} v{1} in {2}" -f $ShortName, $FileVersion, "$MSIDir\$ShortName-$FileVersion.msi")
}

Start-Sleep 3