#region Using directives
using System;
using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Linq;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
//using Microsoft.Win32;
//using System.Reflection;
//using System.Security.Permissions;
//using SAPDataProvider;
//using SAPTableFactoryCtrl;
//using System.Data.OleDb;

#endregion


namespace RouteSplit.TypesXXX
{
    public class RSTConfig
    {
        public DateTime exdat { get; set; }
    }

    //

    public class RSTWerks
    {
        public string werksId { get; set; }
        public string name1 { get; set; }
        public int    zone  { get; set; }

        public RSTWerks(string werksId, string name1, int zone)
        {
            this.werksId = werksId;
            this.name1 = name1;
            this.zone  = zone;
        }
    }

    public class RSTWerksKey : Tuple<string>
    {
        public RSTWerksKey(RSTWerks werks) : base(werks.werksId) { }
        public RSTWerksKey(string werks) : base(werks) { }
        public static implicit operator RSTWerksKey(RSTWerks werks) {
            return new RSTWerksKey(werks);
        }
    }

    public class RSTWerksTab : KeyedCollection<RSTWerksKey, RSTWerks>
    {
        public RSTWerksTab() : base() {}
        protected override RSTWerksKey GetKeyForItem(RSTWerks werks) {
            return werks;
        }
    }

    //

    public class RSTWave
    {
        public string waveId { get; set; }
        public string text { get; set; }
        public bool kardexCheck { get; set; }

        public RSTWave(string waveId, string text, bool kardexCheck)
        {
            this.waveId = waveId;
            this.text = text;
            this.kardexCheck = kardexCheck;
        }
    }

    public class RSTWaveKey : Tuple<string>
    {
        public RSTWaveKey(RSTWave wave) : base(wave.waveId) { }
        public RSTWaveKey(string waveId) : base(waveId) { }
        public static implicit operator RSTWaveKey(RSTWave wave) {
            return new RSTWaveKey(wave);
        }
    }

    public class RSTWavesTab : KeyedCollection<RSTWaveKey, RSTWave>
    {
        public RSTWavesTab() : base() { }
        protected override RSTWaveKey GetKeyForItem(RSTWave wave)
        {
            return wave;
        }
    }

    //

    public class RSTTemplate
    {
        public RSTWerks werksP { get; set; }
        public string templateName { get; set; }
        public RSTWave wave { get; set; }
        public string text { get; set; }

        public RSTTemplate(RSTWerks werksP, string templateName, RSTWave wave, string text)
        {
            this.werksP = werksP;
            this.templateName = templateName;
            this.wave = wave;
            this.text = text;
        }
    }

    public class RSTTemplateKey : Tuple<RSTWerks, string>
    {
        public RSTTemplateKey(RSTTemplate template) : base(template.werksP, template.templateName) { }
        public RSTTemplateKey(RSTWerks werksP, string templateName) : base(werksP, templateName) { }

        public static implicit operator RSTTemplateKey(RSTTemplate template)
        {
            return new RSTTemplateKey(template);
        }
    }

    public class RSTTemplatesTab : KeyedCollection<RSTTemplateKey, RSTTemplate>
    {
        public RSTTemplatesTab() : base() { }
        protected override RSTTemplateKey GetKeyForItem(RSTTemplate template)
        {
            return template;
        }
    }

    //

    public class RSTVPTypeGroup
    {
        public string vpTypeGroupId { get; set; }

        public RSTVPTypeGroup(string vpTypeGroupId)
        {
            this.vpTypeGroupId = vpTypeGroupId;
        }
    }

    public class RSTVPTypeGroupKey : Tuple<string>
    {
        public RSTVPTypeGroupKey(RSTVPTypeGroup vpTypeGroup) : base(vpTypeGroup.vpTypeGroupId) { }
        public RSTVPTypeGroupKey(string vpTypeGroupId) : base(vpTypeGroupId) { }
        public static implicit operator RSTVPTypeGroupKey(RSTVPTypeGroup vpTypeGroup)
        {
            return new RSTVPTypeGroupKey(vpTypeGroup);
        }
    }

    public class RSTVPTypeGroupTab : KeyedCollection<RSTVPTypeGroupKey, RSTVPTypeGroup>
    {
        public RSTVPTypeGroupTab() : base() { }
        protected override RSTVPTypeGroupKey GetKeyForItem(RSTVPTypeGroup vpTypeGroup)
        {
            return vpTypeGroup;
        }
    }

    //

    public class RSTVPTypeGroupConfig
    {
        public RSTWerks werksS { get; set; }
        public RSTVPTypeGroup vpTypeGroup { get; set; }
        public string text { get; set; }

        public RSTVPTypeGroupConfig(RSTWerks werksS, RSTVPTypeGroup vpTypeGroup, string text)
        {
            this.werksS = werksS;
            this.vpTypeGroup = vpTypeGroup;
            this.text = text;
        }
    }

    public class RSTVPTypeGroupConfigKey : Tuple<RSTWerks, RSTVPTypeGroup>
    {
        public RSTVPTypeGroupConfigKey(RSTVPTypeGroupConfig vpTypeGroupConfig) : base(vpTypeGroupConfig.werksS, vpTypeGroupConfig.vpTypeGroup) { }
        public RSTVPTypeGroupConfigKey(RSTWerks werksS, RSTVPTypeGroup vpTypeGroup) : base(werksS, vpTypeGroup) { }
        public static implicit operator RSTVPTypeGroupConfigKey(RSTVPTypeGroupConfig vpTypeGroupConfig)
        {
            return new RSTVPTypeGroupConfigKey(vpTypeGroupConfig);
        }
    }

    public class RSTVPTypeGroupConfigTab : KeyedCollection<RSTVPTypeGroupConfigKey, RSTVPTypeGroupConfig>
    {
        public RSTVPTypeGroupConfigTab() : base() { }
        protected override RSTVPTypeGroupConfigKey GetKeyForItem(RSTVPTypeGroupConfig vpTypeGroupConfig)
        {
            return vpTypeGroupConfig;
        }
    }

    //


    public class RSTVPType
    {
        public string vpTypeId { get; set; }
        public string text { get; set; }
        public bool dummyType { get; set; }

        public RSTVPType(string vpTypeId, string text, bool dummyType)
        {
            this.vpTypeId = vpTypeId;
            this.text = text;
            this.dummyType = dummyType;
        }
    }

    public class RSTVPTypeKey : Tuple<string>
    {
        public RSTVPTypeKey(RSTVPType vpType) : base(vpType.vpTypeId) { }
        public RSTVPTypeKey(string vpTypeId) : base(vpTypeId) { }
        public static implicit operator RSTVPTypeKey(RSTVPType vpType)
        {
            return new RSTVPTypeKey(vpType);
        }
    }

    public class RSTVPTypeTab : KeyedCollection<RSTVPTypeKey, RSTVPType>
    {
        public RSTVPTypeTab() : base() { }
        protected override RSTVPTypeKey GetKeyForItem(RSTVPType vpType)
        {
            return vpType;
        }
    }


    //

    public class RSTVPTypeGroupItem
    {
        public RSTVPTypeGroup vpTypeGroup { get; set; }
        public RSTVPType vpType { get; set; }
        public int defaultSeq { get; set; }
        public bool defaultSel { get; set; }
        public string text { get; set; }

        public RSTVPTypeGroupItem(RSTVPTypeGroup vpTypeGroup, RSTVPType vpType, int defaultSeq, bool defaultSel, string text)
        {
            this.vpTypeGroup = vpTypeGroup;
            this.vpType = vpType;
            this.defaultSeq = defaultSeq;
            this.defaultSel = defaultSel;
            this.text = text;
        }
    }

    public class RSTVPTypeGroupItemKey : Tuple<RSTVPTypeGroup, RSTVPType>
    {
        public RSTVPTypeGroupItemKey(RSTVPTypeGroupItem vpTypeGroupItem) : base(vpTypeGroupItem.vpTypeGroup, vpTypeGroupItem.vpType) { }
        public RSTVPTypeGroupItemKey(RSTVPTypeGroup vpTypeGroup, RSTVPType vpType) : base(vpTypeGroup, vpType) { }
        public static implicit operator RSTVPTypeGroupItemKey(RSTVPTypeGroupItem vpTypeGroupItem)
        {
            return new RSTVPTypeGroupItemKey(vpTypeGroupItem);
        }
    }

    public class RSTVPTypeGroupItemTab : KeyedCollection<RSTVPTypeGroupItemKey, RSTVPTypeGroupItem>
    {
        public RSTVPTypeGroupItemTab() : base() { }
        protected override RSTVPTypeGroupItemKey GetKeyForItem(RSTVPTypeGroupItem vpTypeGroupItem)
        {
            return vpTypeGroupItem;
        }
    }

    //

    public class RSTVPTypeGroupItemConfig
    {
        public RSTVPTypeGroupConfig vpTypeGroupConfig { get; set; }
        public RSTVPType vpType { get; set; }

        public bool available { get; set; }
        public int currentSeq { get; set; }
        public bool currentSel { get; set; }

        public RSTVPTypeGroupItemConfig(RSTVPTypeGroupConfig vpTypeGroupConfig, RSTVPType vpType, int currentSeq, bool currentSel, bool available)
        {
            this.vpTypeGroupConfig = vpTypeGroupConfig;
            this.vpType = vpType;
            this.available = available;
            this.currentSeq = currentSeq;
            this.currentSel = currentSel;
        }
    }

    public class RSTVPTypeGroupItemConfigKey : Tuple<RSTVPTypeGroupConfig, RSTVPType>
    {
        public RSTVPTypeGroupItemConfigKey(RSTVPTypeGroupItemConfig vpTypeGroupItemConfig) : base(vpTypeGroupItemConfig.vpTypeGroupConfig, vpTypeGroupItemConfig.vpType) { }
        public RSTVPTypeGroupItemConfigKey(RSTVPTypeGroupConfig vpTypeGroupConfig, RSTVPType vpType) : base(vpTypeGroupConfig, vpType) { }
        public static implicit operator RSTVPTypeGroupItemConfigKey(RSTVPTypeGroupItemConfig vpTypeGroupItemConfig)
        {
            return new RSTVPTypeGroupItemConfigKey(vpTypeGroupItemConfig);
        }
    }

    public class RSTVPTypeGroupItemConfigTab : KeyedCollection<RSTVPTypeGroupItemConfigKey, RSTVPTypeGroupItemConfig>
    {
        public RSTVPTypeGroupItemConfigTab() : base() { }
        protected override RSTVPTypeGroupItemConfigKey GetKeyForItem(RSTVPTypeGroupItemConfig vpTypeGroupItemConfig)
        {
            return vpTypeGroupItemConfig;
        }
    }

    //

    public class RSTProcess
    {
        public RSTTemplate template { get; set; }
        public int seqNo { get; set; }
        public DateTime shipDate { get; set; }
        public bool extract { get; set; }
        public bool unassigned { get; set; }

        public RSTProcess(RSTTemplate template, int seqNo, DateTime shipDate, bool extract, bool unassigned)
        {
            this.template = template;
            this.seqNo = seqNo;
            this.shipDate = shipDate;
            this.extract = extract;
            this.unassigned = unassigned;
        }
    }

    public class RSTProcessKey : Tuple<RSTTemplate, int, DateTime>
    {
        public RSTProcessKey(RSTTemplate template, int seqNo, DateTime shipDate) : base(template, seqNo, shipDate) { }
        public RSTProcessKey(RSTProcess process) : base(process.template, process.seqNo, process.shipDate) { }
        public static implicit operator RSTProcessKey(RSTProcess process)
        {
            return new RSTProcessKey(process);
        }
    }

    public class RSTProcessTab : KeyedCollection<RSTProcessKey, RSTProcess>
    {
        public RSTProcessTab() : base() { }
        protected override RSTProcessKey GetKeyForItem(RSTProcess process)
        {
            return process;
        }
    }

    //

    public class RSTRoute
    {
        public RSTWerks werksS { get; set; }
        public string routeId { get; set; }
        public string text { get; set; }
        public RSTVPType vpType { get; set; }

        public RSTRoute(RSTWerks werksS, string routeId, string text, RSTVPType vpType)
        {
            this.werksS = werksS;
            this.routeId = routeId;
            this.text = text;
            this.vpType = vpType;
        }
    }

    public class RSTRouteKey : Tuple<RSTWerks, string>
    {
        public RSTRouteKey(RSTWerks werksS, string routeId) : base(werksS, routeId) { }
        public RSTRouteKey(RSTRoute route) : base(route.werksS, route.routeId) { }
        public static implicit operator RSTRouteKey(RSTRoute route)
        {
            return new RSTRouteKey(route);
        }
    }

    public class RSTRouteTab : KeyedCollection<RSTRouteKey, RSTRoute>
    {
        public RSTRouteTab() : base() { }
        protected override RSTRouteKey GetKeyForItem(RSTRoute route)
        {
            return route;
        }
    }

    //

    public class RSTKunwe
    {
        public string kunweId { get; set; }
        public RSTWerks werksS { get; set; }
        public string kardex { get; set; }

        public RSTKunwe(string kunweId, RSTWerks werksS, string kardex)
        {
            this.kunweId = kunweId;
            this.werksS = werksS;
            this.kardex = kardex;
        }
    }

    public class RSTKunweKey : Tuple<string>
    {
        public RSTKunweKey(string kunweId) : base(kunweId) { }
        public RSTKunweKey(RSTKunwe kunwe) : base(kunwe.kunweId) { }
        public static implicit operator RSTKunweKey(RSTKunwe kunwe)
        {
            return new RSTKunweKey(kunwe);
        }
    }

    public class RSTKunweTab : KeyedCollection<RSTKunweKey, RSTKunwe>
    {
        public RSTKunweTab() : base() { }
        protected override RSTKunweKey GetKeyForItem(RSTKunwe kunwe)
        {
            return kunwe;
        }
    }

    //

    public class RSTDrop
    {
        public RSTRoute route { get; set; }
        public RSTKunwe kunwe { get; set; }
        public int sequ { get; set; }

        public RSTDrop(RSTRoute route, RSTKunwe kunwe, int sequ)
        {
            this.route = route;
            this.kunwe = kunwe;
            this.sequ = sequ;
        }
    }

    public class RSTDropKey : Tuple<RSTRoute, RSTKunwe>
    {
        public RSTDropKey(RSTRoute route, RSTKunwe kunwe) : base(route, kunwe) { }
        public RSTDropKey(RSTDrop drop) : base(drop.route, drop.kunwe) { }
        public static implicit operator RSTDropKey(RSTDrop drop)
        {
            return new RSTDropKey(drop);
        }
    }

    public class RSTDropTab : KeyedCollection<RSTDropKey, RSTDrop>
    {
        public RSTDropTab() : base() { }
        protected override RSTDropKey GetKeyForItem(RSTDrop drop)
        {
            return drop;
        }
    }

    //

    public class RSTIssue
    {
        public string issueId { get; set; }
        public string text { get; set; }
        public RSTIssue(string issueId, string text)
        {
            this.issueId = issueId;
            this.text = text;
        }
    }

    public class RSTIssueKey : Tuple<string>
    {
        public RSTIssueKey(string issueId) : base(issueId) { }
        public RSTIssueKey(RSTIssue issue) : base(issue.issueId) { }
        public static implicit operator RSTIssueKey(RSTIssue issue)
        {
            return new RSTIssueKey(issue);
        }
    }

    public class RSTIssueTab : KeyedCollection<RSTIssueKey, RSTIssue>
    {
        public RSTIssueTab() : base() { }
        protected override RSTIssueKey GetKeyForItem(RSTIssue issue)
        {
            return issue;
        }
    }

    //

    public class RSTIssueWerks
    {
        public RSTIssue issue { get; set; }
        public RSTWerks werksC { get; set; }

        public int weight { get; set; }
        public int bunSize { get; set; }
        public int bunWeight { get; set; }

        public RSTIssueWerks(RSTIssue issue, RSTWerks werksC, int weight, int bunSize, int bunWeight)
        {
            this.issue = issue;
            this.werksC = werksC;

            this.weight = weight;
            this.bunSize = bunSize;
            this.bunWeight = bunWeight;
        }
    }

    public class RSTIssueWerksKey : Tuple<RSTIssue, RSTWerks>
    {
        public RSTIssueWerksKey(RSTIssue issue, RSTWerks werksC) : base(issue, werksC) { }
        public RSTIssueWerksKey(RSTIssueWerks issueWerks) : base(issueWerks.issue, issueWerks.werksC) { }
        public static implicit operator RSTIssueWerksKey(RSTIssueWerks issueWerks)
        {
            return new RSTIssueWerksKey(issueWerks);
        }
    }

    public class RSTIssueWerksTab : KeyedCollection<RSTIssueWerksKey, RSTIssueWerks>
    {
        public RSTIssueWerksTab() : base() { }
        protected override RSTIssueWerksKey GetKeyForItem(RSTIssueWerks issueWerks)
        {
            return issueWerks;
        }
    }

    //

    public class RSTContract
    {
        public RSTKunwe kunwe { get; set; }
        public RSTIssue issue { get; set; }
        public RSTWerks werksC { get; set; }

        public RSTContract(RSTKunwe kunwe, RSTIssue issue, RSTWerks werksc)
        {
            this.kunwe = kunwe;
            this.issue = issue;
            this.werksC = werksC;
        }
    }

    public class RSTContractKey : Tuple<RSTKunwe, RSTIssue>
    {
        public RSTContractKey(RSTKunwe kunwe, RSTIssue issue) : base(kunwe, issue) { }
        public RSTContractKey(RSTContract contract) : base(contract.kunwe, contract.issue) { }
        public static implicit operator RSTContractKey(RSTContract contract)
        {
            return new RSTContractKey(contract);
        }
    }

    public class RSTContractTab : KeyedCollection<RSTContractKey, RSTContract>
    {
        public RSTContractTab() : base() { }
        protected override RSTContractKey GetKeyForItem(RSTContract contract)
        {
            return contract;
        }
    }

    //

    public class RSTIssuePhase
    {
        public RSTIssue issue { get; set; }
        public string phaseModel { get; set; }
        public int phaseNo { get; set; }

        public RSTIssuePhase(RSTIssue issue, string phaseModel, int phaseNo)
        {
            this.issue = issue;
            this.phaseModel = phaseModel;
            this.phaseNo = phaseNo;
        }
    }

    public class RSTIssuePhaseKey : Tuple<RSTIssue, string, int>
    {
        public RSTIssuePhaseKey(RSTIssue issue, string phaseModel, int phaseNo) : base(issue, phaseModel, phaseNo) { }
        public RSTIssuePhaseKey(RSTIssuePhase phase) : base(phase.issue, phase.phaseModel, phase.phaseNo) { }
        public static implicit operator RSTIssuePhaseKey(RSTIssuePhase phase)
        {
            return new RSTIssuePhaseKey(phase);
        }
    }

    public class RSTIssuePhaseTab : KeyedCollection<RSTIssuePhaseKey, RSTIssuePhase>
    {
        public RSTIssuePhaseTab() : base() { }
        protected override RSTIssuePhaseKey GetKeyForItem(RSTIssuePhase phase)
        {
            return phase;
        }
    }

    //

    public class RSTPackPhase
    {
        public RSTIssuePhase phase { get; set; }
        public RSTWerks werksP { get; set; }
        public bool active { get; set; }
        public RSTWave wave { get; set; }

        public RSTPackPhase(RSTIssuePhase phase, RSTWerks werksP, bool active, RSTWave wave)
        {
            this.phase = phase;
            this.werksP = werksP;
            this.active = active;
            this.wave = wave;
        }
    }

    public class RSTPackPhaseKey : Tuple<RSTIssuePhase, RSTWerks>
    {
        public RSTPackPhaseKey(RSTIssuePhase phase, RSTWerks werksP) : base(phase, werksP) { }
        public RSTPackPhaseKey(RSTPackPhase pkPhase) : base(pkPhase.phase, pkPhase.werksP) { }
        public static implicit operator RSTPackPhaseKey(RSTPackPhase pkPhase)
        {
            return new RSTPackPhaseKey(pkPhase);
        }
    }

    public class RSTPackPhaseTab : KeyedCollection<RSTPackPhaseKey, RSTPackPhase>
    {
        public RSTPackPhaseTab() : base() { }
        protected override RSTPackPhaseKey GetKeyForItem(RSTPackPhase pkPhase)
        {
            return pkPhase;
        }
    }

    //

    public class RSTPhaseProcess
    {
        public RSTIssuePhase phase { get; set; }
        public RSTProcess process { get; set; }

        public bool assigned { get; set; }
        public bool extracted { get; set; }

        public RSTPhaseProcess(RSTIssuePhase phase, RSTProcess process, bool assigned, bool extracted)
        {
            this.phase = phase;
            this.process = process;
            this.assigned = assigned;
            this.extracted = extracted;
        }
    }

    public class RSTPhaseProcessKey : Tuple<RSTIssuePhase, RSTProcess>
    {
        public RSTPhaseProcessKey(RSTIssuePhase phase, RSTProcess process) : base(phase, process) { }
        public RSTPhaseProcessKey(RSTPhaseProcess phaseProcess) : base(phaseProcess.phase, phaseProcess.process) { }
        public static implicit operator RSTPhaseProcessKey(RSTPhaseProcess phaseProcess)
        {
            return new RSTPhaseProcessKey(phaseProcess);
        }
    }

    public class RSTPhaseProcessTab : KeyedCollection<RSTPhaseProcessKey, RSTPhaseProcess>
    {
        public RSTPhaseProcessTab() : base() { }
        protected override RSTPhaseProcessKey GetKeyForItem(RSTPhaseProcess phaseProcess)
        {
            return phaseProcess;
        }
    }
    //

    public class RSTDemand
    {
        public RSTIssuePhase phase { get; set; }
        public RSTKunwe kunwe { get; set; }

        public int copies { get; set; }
        public int picks { get; set; }
        public int weight { get; set; }
        public RSTWerks origWerksC { get; set; }

        /* derived from model (cached for performance) */
        public RSTWerks werksP;
        public RSTDrop drop;
        public RSTProcess process;

        public RSTDemand(RSTIssuePhase phase, RSTKunwe kunwe, int copies, int picks, int weight, RSTWerks origWerksC)
        {
            this.phase = phase;
            this.kunwe = kunwe;
            this.copies = copies;
            this.picks = picks;
            this.weight = weight;
            this.origWerksC = origWerksC;

            /* FIXME - derive additional properties */
        }
    }

    public class RSTDemandKey : Tuple<RSTIssuePhase, RSTKunwe>
    {
        public RSTDemandKey(RSTIssuePhase phase, RSTKunwe kunwe) : base(phase, kunwe) { }
        public RSTDemandKey(RSTDemand demand) : base(demand.phase, demand.kunwe) { }
        public static implicit operator RSTDemandKey(RSTDemand demand)
        {
            return new RSTDemandKey(demand);
        }
    }

    public class RSTDemandTab : KeyedCollection<RSTDemandKey, RSTDemand>
    {
        public RSTDemandTab() : base() { }
        protected override RSTDemandKey GetKeyForItem(RSTDemand demand)
        {
            return demand;
        }
    }

    //

    public class RSTSubRoute
    {
        public RSTRoute route { get; set; }
        public string subRouteId { get; set; }

        public RSTWerks werksC { get; set; }
        public RSTWerks origWerksP { get; set; }
        public RSTProcess process { get; set; }

        /* FIXME - summary fields */

        public RSTSubRoute(RSTRoute route, string subRouteId, RSTWerks werksC, RSTWerks origWerksP, RSTProcess process)
        {
            this.route = route;
            this.subRouteId = subRouteId;

            this.werksC = werksC;
            this.origWerksP = origWerksP;
            this.process = process;
        }
    }

    public class RSTSubRouteKey : Tuple<RSTRoute, string>
    {
        public RSTSubRouteKey(RSTRoute route, string subRouteId) : base(route, subRouteId) { }
        public RSTSubRouteKey(RSTSubRoute subRoute) : base(subRoute.route, subRoute.subRouteId) { }
        public static implicit operator RSTSubRouteKey(RSTSubRoute subRoute)
        {
            return new RSTSubRouteKey(subRoute);
        }
    }

    public class RSTSubRouteTab : KeyedCollection<RSTSubRouteKey, RSTSubRoute>
    {
        public RSTSubRouteTab() : base() { }
        protected override RSTSubRouteKey GetKeyForItem(RSTSubRoute subRoute)
        {
            return subRoute;
        }
    }

    public class RSTState
    {
        public RSTConfig Config { get; set; }
        public RSTWerksTab WerksTab { get; set; }
        public RSTWavesTab WavesTab { get; set; }
        public RSTTemplatesTab TemplatesTab { get; set; }
        public RSTVPTypeGroupTab VPTypeGroupTab { get; set; }
        public RSTVPTypeGroupConfigTab VPTypeGroupConfigTab { get; set; }
        public RSTVPTypeGroupItemTab VPTypeGroupItemTab { get; set; }
        public RSTVPTypeGroupItemConfigTab VPTypeGroupItemConfigTab { get; set; }
        public RSTProcessTab ProcessTab { get; set; }
        public RSTVPTypeTab VPTypeTab { get; set; }
        public RSTRouteTab RouteTab { get; set; }
        public RSTDropTab DropTab { get; set; }
        public RSTKunweTab KunweTab { get; set; }
        public RSTContractTab ContractTab { get; set; }
        public RSTPhaseProcessTab PhaseProcessTab { get; set; }
        public RSTDemandTab DemandTab { get; set; }
        public RSTPackPhaseTab PackPhaseTab { get; set; }
        public RSTIssuePhaseTab IssuePhaseTab { get; set; }
        public RSTSubRouteTab SubRouteTab { get; set; }
        public RSTIssueTab IssueTab { get; set; }
        public RSTIssueWerksTab IssueWerksTab { get; set; }

        public RSTState()
        {
            Config = new RSTConfig();
            WerksTab = new RSTWerksTab();
            WavesTab = new RSTWavesTab();
            TemplatesTab = new RSTTemplatesTab();
            VPTypeGroupTab = new RSTVPTypeGroupTab();
            VPTypeGroupConfigTab = new RSTVPTypeGroupConfigTab();
            VPTypeGroupItemTab = new RSTVPTypeGroupItemTab();
            VPTypeGroupItemConfigTab = new RSTVPTypeGroupItemConfigTab();
            ProcessTab = new RSTProcessTab();
            VPTypeTab = new RSTVPTypeTab();
            RouteTab = new RSTRouteTab();
            DropTab = new RSTDropTab();
            KunweTab = new RSTKunweTab();
            ContractTab = new RSTContractTab();
            PhaseProcessTab = new RSTPhaseProcessTab();
            DemandTab = new RSTDemandTab();
            PackPhaseTab = new RSTPackPhaseTab();
            IssuePhaseTab = new RSTIssuePhaseTab();
            SubRouteTab = new RSTSubRouteTab();
            IssueTab = new RSTIssueTab();
            IssueWerksTab = new RSTIssueWerksTab();            
        }

    }

}