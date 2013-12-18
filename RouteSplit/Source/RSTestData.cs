using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using RouteSplit.Types;

namespace RSTestData
{
    public static class RSTestData
    {
        public static RSTState InitData()
        {
            RSTState State = new RSTState();

            State.Config.exdat = new DateTime(2013, 12, 18);

            State.WerksTab.Add(new RSTWerks("310", "PRESTON", 0));
            State.WerksTab.Add(new RSTWerks("410", "KENDAL", 1));
            State.WerksTab.Add(new RSTWerks("440", "CARLISLE", 2));

            State.WavesTab.Add(new RSTWave("01", "HS", false));
            State.WavesTab.Add(new RSTWave("02", "KARDEX", true));
            State.WavesTab.Add(new RSTWave("03", "WORKSTATION NEWS", false));
            State.WavesTab.Add(new RSTWave("04", "WORKSTATION AD HOC", false));

            State.WavesTab.Add(new RSTWave("99", "DUMMY", false)); // FIXME - move dummy flag from process to wave ??????

            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("310")], "HS01", State.WavesTab[new RSTWaveKey("01")], "HS 1ST PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("310")], "HS02", State.WavesTab[new RSTWaveKey("01")], "HS 2ND PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("310")], "HS03", State.WavesTab[new RSTWaveKey("01")], "HS 3RD PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("310")], "WS01", State.WavesTab[new RSTWaveKey("03")], "WORKSTATION NEWS PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("310")], "WS02", State.WavesTab[new RSTWaveKey("03")], "WORKSTATION NEWS PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("310")], "WA01", State.WavesTab[new RSTWaveKey("04")], "WORKSTATION AD HOC"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("310")], "WA02", State.WavesTab[new RSTWaveKey("04")], "WORKSTATION AD HOC"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("310")], "WA03", State.WavesTab[new RSTWaveKey("04")], "WORKSTATION AD HOC"));

            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("410")], "WS01", State.WavesTab[new RSTWaveKey("03")], "WORKSTATION NEWS PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("410")], "WS02", State.WavesTab[new RSTWaveKey("03")], "WORKSTATION NEWS PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("410")], "WA01", State.WavesTab[new RSTWaveKey("04")], "WORKSTATION AD HOC"));

            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("440")], "KA01", State.WavesTab[new RSTWaveKey("02")], "KARDEX 1ST PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("440")], "WS01", State.WavesTab[new RSTWaveKey("03")], "WORKSTATION NEWS PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("440")], "WS02", State.WavesTab[new RSTWaveKey("03")], "WORKSTATION NEWS PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("440")], "WS03", State.WavesTab[new RSTWaveKey("03")], "WORKSTATION NEWS PACK"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("440")], "WA01", State.WavesTab[new RSTWaveKey("04")], "WORKSTATION AD HOC"));

            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("310")], "ZZ99", State.WavesTab[new RSTWaveKey("99")], "DUMMY"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("410")], "ZZ99", State.WavesTab[new RSTWaveKey("99")], "DUMMY"));
            State.TemplatesTab.Add(new RSTTemplate(State.WerksTab[new RSTWerksKey("440")], "ZZ99", State.WavesTab[new RSTWaveKey("99")], "DUMMY"));

            State.VPTypeGroupTab.Add(new RSTVPTypeGroup("A"));
            State.VPTypeGroupTab.Add(new RSTVPTypeGroup("D"));

            State.VPTypeGroupConfigTab.Add(new RSTVPTypeGroupConfig(State.WerksTab[new RSTWerksKey("310")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")], "DAY/PRE/MAIN"));
            State.VPTypeGroupConfigTab.Add(new RSTVPTypeGroupConfig(State.WerksTab[new RSTWerksKey("310")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("D")], "MAIN RUNS"));
            State.VPTypeGroupConfigTab.Add(new RSTVPTypeGroupConfig(State.WerksTab[new RSTWerksKey("410")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")], "DAY/PRE/MAIN"));
            State.VPTypeGroupConfigTab.Add(new RSTVPTypeGroupConfig(State.WerksTab[new RSTWerksKey("410")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("D")], "MAIN RUNS"));
            State.VPTypeGroupConfigTab.Add(new RSTVPTypeGroupConfig(State.WerksTab[new RSTWerksKey("440")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")], "DAY/PRE/MAIN"));
            State.VPTypeGroupConfigTab.Add(new RSTVPTypeGroupConfig(State.WerksTab[new RSTWerksKey("440")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("D")], "MAIN RUNS"));

            State.VPTypeTab.Add(new RSTVPType("01", "Main Run Deliveries", false));
            State.VPTypeTab.Add(new RSTVPType("03", "Pre-Run Deliveries", false));
            State.VPTypeTab.Add(new RSTVPType("04", "Day Run Deliveries", false));
            State.VPTypeTab.Add(new RSTVPType("05", "English Title Run Deliveries", false));
            State.VPTypeTab.Add(new RSTVPType("99", "Missing Visit Lists", true));

            State.VPTypeGroupItemTab.Add(new RSTVPTypeGroupItem(State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")], State.VPTypeTab[new RSTVPTypeKey("01")], 3, true, "MAIN RUNS"));
            State.VPTypeGroupItemTab.Add(new RSTVPTypeGroupItem(State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")], State.VPTypeTab[new RSTVPTypeKey("03")], 2, true, "PRE RUNS"));
            State.VPTypeGroupItemTab.Add(new RSTVPTypeGroupItem(State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")], State.VPTypeTab[new RSTVPTypeKey("04")], 1, false, "DAY RUNS"));
            State.VPTypeGroupItemTab.Add(new RSTVPTypeGroupItem(State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")], State.VPTypeTab[new RSTVPTypeKey("05")], 4, true, "ENG RUNS"));

            State.VPTypeGroupItemTab.Add(new RSTVPTypeGroupItem(State.VPTypeGroupTab[new RSTVPTypeGroupKey("D")], State.VPTypeTab[new RSTVPTypeKey("01")], 1, true, "MAIN RUNS"));

            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("310")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("01")], 3, true, true));
            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("310")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("03")], 2, true, true));
            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("310")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("04")], 1, false, true));
            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("310")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("05")], 4, true, true));

            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("310")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("D")])], State.VPTypeTab[new RSTVPTypeKey("01")], 1, true, true));

            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("410")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("01")], 3, true, true));
            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("410")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("03")], 2, true, true));
            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("410")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("04")], 1, false, true));
            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("410")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("05")], 4, true, true));

            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("410")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("D")])], State.VPTypeTab[new RSTVPTypeKey("01")], 1, true, true));

            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("440")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("01")], 3, true, true));
            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("440")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("03")], 2, true, true));
            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("440")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("04")], 1, false, true));
            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("440")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("A")])], State.VPTypeTab[new RSTVPTypeKey("05")], 4, true, true));

            State.VPTypeGroupItemConfigTab.Add(new RSTVPTypeGroupItemConfig(State.VPTypeGroupConfigTab[new RSTVPTypeGroupConfigKey(State.WerksTab[new RSTWerksKey("440")], State.VPTypeGroupTab[new RSTVPTypeGroupKey("D")])], State.VPTypeTab[new RSTVPTypeKey("01")], 1, true, true));

            State.ProcessTab.Add(new RSTProcess(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("310")], "ZZ99")], 0, new DateTime(4000, 12, 31), false, true));
            State.ProcessTab.Add(new RSTProcess(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("410")], "ZZ99")], 0, new DateTime(4000, 12, 31), false, true));
            State.ProcessTab.Add(new RSTProcess(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("440")], "ZZ99")], 0, new DateTime(4000, 12, 31), false, true));

            State.ProcessTab.Add(new RSTProcess(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("310")], "HS01")], 1, new DateTime(2013, 12, 18), true, false));
            State.ProcessTab.Add(new RSTProcess(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("310")], "WS01")], 1, new DateTime(2013, 12, 18), true, false));
            State.ProcessTab.Add(new RSTProcess(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("410")], "WS01")], 1, new DateTime(2013, 12, 18), true, false));
            State.ProcessTab.Add(new RSTProcess(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("440")], "WS01")], 1, new DateTime(2013, 12, 18), true, false));
            State.ProcessTab.Add(new RSTProcess(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("440")], "KA01")], 1, new DateTime(2013, 12, 18), true, false));


            State.RouteTab.Add(new RSTRoute(State.WerksTab[new RSTWerksKey("310")], "310Z99", "MISSING", State.VPTypeTab[new RSTVPTypeKey("99")]));
            State.RouteTab.Add(new RSTRoute(State.WerksTab[new RSTWerksKey("410")], "410Z99", "MISSING", State.VPTypeTab[new RSTVPTypeKey("99")]));
            State.RouteTab.Add(new RSTRoute(State.WerksTab[new RSTWerksKey("440")], "440Z99", "MISSING", State.VPTypeTab[new RSTVPTypeKey("99")]));
            // FIXME - add routes

            State.KunweTab.Add(new RSTKunwe("310310", State.WerksTab[new RSTWerksKey("310")], ""));
            State.KunweTab.Add(new RSTKunwe("410410", State.WerksTab[new RSTWerksKey("410")], ""));
            State.KunweTab.Add(new RSTKunwe("440440", State.WerksTab[new RSTWerksKey("440")], ""));
            // FIXME - add customers

            State.DropTab.Add(new RSTDrop(State.RouteTab[new RSTRouteKey(State.WerksTab[new RSTWerksKey("310")], "310Z99")], State.KunweTab[new RSTKunweKey("310310")], 1));
            State.DropTab.Add(new RSTDrop(State.RouteTab[new RSTRouteKey(State.WerksTab[new RSTWerksKey("410")], "410Z99")], State.KunweTab[new RSTKunweKey("410410")], 1));
            State.DropTab.Add(new RSTDrop(State.RouteTab[new RSTRouteKey(State.WerksTab[new RSTWerksKey("440")], "440Z99")], State.KunweTab[new RSTKunweKey("440440")], 1));
            // FIXME - add drops

            State.SubRouteTab.Add(new RSTSubRoute(State.RouteTab[new RSTRouteKey(State.WerksTab[new RSTWerksKey("310")], "310Z99")], "", null, null, State.ProcessTab[new RSTProcessKey(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("310")], "ZZ99")], 0, new DateTime(4000, 12, 31))]));
            State.SubRouteTab.Add(new RSTSubRoute(State.RouteTab[new RSTRouteKey(State.WerksTab[new RSTWerksKey("410")], "410Z99")], "", null, null, State.ProcessTab[new RSTProcessKey(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("410")], "ZZ99")], 0, new DateTime(4000, 12, 31))]));
            State.SubRouteTab.Add(new RSTSubRoute(State.RouteTab[new RSTRouteKey(State.WerksTab[new RSTWerksKey("440")], "440Z99")], "", null, null, State.ProcessTab[new RSTProcessKey(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("440")], "ZZ99")], 0, new DateTime(4000, 12, 31))]));
            // FIXME - add SubRoutes

            State.IssueTab.Add(new RSTIssue("310MAG", "MAGAZINE - HUB CONTRACT"));
            State.IssueTab.Add(new RSTIssue("440REG", "REGIONAL - 440 CONTRACT + 410"));
            State.IssueTab.Add(new RSTIssue("310NWS", "NEWSPAPER - SPOKE CONTRACT"));
            State.IssueTab.Add(new RSTIssue("410NWS", "NEWSPAPER - SPOKE CONTRACT"));
            State.IssueTab.Add(new RSTIssue("440NWS", "NEWSPAPER - SPOKE CONTRACT"));
            // FIXME - Issues

            State.ContractTab.Add(new RSTContract(State.KunweTab[new RSTKunweKey("310310")], State.IssueTab[new RSTIssueKey("310MAG")], State.WerksTab[new RSTWerksKey("310")]));
            State.ContractTab.Add(new RSTContract(State.KunweTab[new RSTKunweKey("410410")], State.IssueTab[new RSTIssueKey("310MAG")], State.WerksTab[new RSTWerksKey("310")]));
            State.ContractTab.Add(new RSTContract(State.KunweTab[new RSTKunweKey("440440")], State.IssueTab[new RSTIssueKey("310MAG")], State.WerksTab[new RSTWerksKey("310")]));
            
            State.ContractTab.Add(new RSTContract(State.KunweTab[new RSTKunweKey("410410")], State.IssueTab[new RSTIssueKey("440REG")], State.WerksTab[new RSTWerksKey("440")]));
            State.ContractTab.Add(new RSTContract(State.KunweTab[new RSTKunweKey("440440")], State.IssueTab[new RSTIssueKey("440REG")], State.WerksTab[new RSTWerksKey("440")]));

            State.ContractTab.Add(new RSTContract(State.KunweTab[new RSTKunweKey("310310")], State.IssueTab[new RSTIssueKey("310NWS")], State.WerksTab[new RSTWerksKey("310")]));
            State.ContractTab.Add(new RSTContract(State.KunweTab[new RSTKunweKey("410410")], State.IssueTab[new RSTIssueKey("410NWS")], State.WerksTab[new RSTWerksKey("410")]));
            State.ContractTab.Add(new RSTContract(State.KunweTab[new RSTKunweKey("440440")], State.IssueTab[new RSTIssueKey("440NWS")], State.WerksTab[new RSTWerksKey("440")]));
            // FIXME - Contracts

            State.IssueWerksTab.Add(new RSTIssueWerks(State.IssueTab[new RSTIssueKey("310MAG")], State.WerksTab[new RSTWerksKey("310")], 101, 21, (101*21+15)));
            State.IssueWerksTab.Add(new RSTIssueWerks(State.IssueTab[new RSTIssueKey("440REG")], State.WerksTab[new RSTWerksKey("440")], 102, 22, (102*22+14)));
            State.IssueWerksTab.Add(new RSTIssueWerks(State.IssueTab[new RSTIssueKey("310NWS")], State.WerksTab[new RSTWerksKey("310")], 103, 23, (103*23+13)));
            State.IssueWerksTab.Add(new RSTIssueWerks(State.IssueTab[new RSTIssueKey("410NWS")], State.WerksTab[new RSTWerksKey("410")], 104, 24, (104*24+12)));
            State.IssueWerksTab.Add(new RSTIssueWerks(State.IssueTab[new RSTIssueKey("440NWS")], State.WerksTab[new RSTWerksKey("440")], 105, 25, (105*25+11)));
            // FIXME - IssueWerks

            State.IssuePhaseTab.Add(new RSTIssuePhase(State.IssueTab[new RSTIssueKey("310MAG")], "", 0));
            State.IssuePhaseTab.Add(new RSTIssuePhase(State.IssueTab[new RSTIssueKey("440REG")], "", 0));
            State.IssuePhaseTab.Add(new RSTIssuePhase(State.IssueTab[new RSTIssueKey("310NWS")], "", 0));
            State.IssuePhaseTab.Add(new RSTIssuePhase(State.IssueTab[new RSTIssueKey("410NWS")], "", 0));
            State.IssuePhaseTab.Add(new RSTIssuePhase(State.IssueTab[new RSTIssueKey("440NWS")], "", 0));
            // FIXME - IssuePhase

            State.DemandTab.Add(new RSTDemand(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("310MAG")], "", 0)], State.KunweTab[new RSTKunweKey("310310")], 1, 1, (1*101), State.WerksTab[new RSTWerksKey("310")]));
            State.DemandTab.Add(new RSTDemand(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("310MAG")], "", 0)], State.KunweTab[new RSTKunweKey("410410")], 1, 1, (1*101), State.WerksTab[new RSTWerksKey("310")]));
            State.DemandTab.Add(new RSTDemand(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("310MAG")], "", 0)], State.KunweTab[new RSTKunweKey("440440")], 1, 1, (1*101), State.WerksTab[new RSTWerksKey("310")]));
            State.DemandTab.Add(new RSTDemand(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("440REG")], "", 0)], State.KunweTab[new RSTKunweKey("410410")], 1, 1, (1*101), State.WerksTab[new RSTWerksKey("440")]));
            State.DemandTab.Add(new RSTDemand(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("440REG")], "", 0)], State.KunweTab[new RSTKunweKey("440440")], 1, 1, (1*101), State.WerksTab[new RSTWerksKey("440")]));
            State.DemandTab.Add(new RSTDemand(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("310NWS")], "", 0)], State.KunweTab[new RSTKunweKey("310310")], 1, 1, (1*101), State.WerksTab[new RSTWerksKey("310")]));
            State.DemandTab.Add(new RSTDemand(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("410NWS")], "", 0)], State.KunweTab[new RSTKunweKey("410410")], 1, 1, (1*101), State.WerksTab[new RSTWerksKey("410")]));
            State.DemandTab.Add(new RSTDemand(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("440NWS")], "", 0)], State.KunweTab[new RSTKunweKey("440440")], 1, 1, (1*101), State.WerksTab[new RSTWerksKey("440")]));
            // FIXME - Demand

            State.PackPhaseTab.Add(new RSTPackPhase(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("310MAG")], "", 0)], State.WerksTab[new RSTWerksKey("310")], true, State.WavesTab[new RSTWaveKey("01")]));
            State.PackPhaseTab.Add(new RSTPackPhase(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("440REG")], "", 0)], State.WerksTab[new RSTWerksKey("440")], true, State.WavesTab[new RSTWaveKey("02")]));
            State.PackPhaseTab.Add(new RSTPackPhase(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("310NWS")], "", 0)], State.WerksTab[new RSTWerksKey("310")], true, State.WavesTab[new RSTWaveKey("03")]));
            State.PackPhaseTab.Add(new RSTPackPhase(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("410NWS")], "", 0)], State.WerksTab[new RSTWerksKey("410")], true, State.WavesTab[new RSTWaveKey("03")]));
            State.PackPhaseTab.Add(new RSTPackPhase(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("440NWS")], "", 0)], State.WerksTab[new RSTWerksKey("440")], true, State.WavesTab[new RSTWaveKey("03")]));
            // FIXME - PackPhase

            State.PhaseProcessTab.Add(new RSTPhaseProcess(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("310MAG")], "", 0)], State.ProcessTab[new RSTProcessKey(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("310")], "ZZ99")], 0, new DateTime(4000, 12, 31))], false, false));
            State.PhaseProcessTab.Add(new RSTPhaseProcess(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("440REG")], "", 0)], State.ProcessTab[new RSTProcessKey(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("440")], "ZZ99")], 0, new DateTime(4000, 12, 31))], false, false));
            State.PhaseProcessTab.Add(new RSTPhaseProcess(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("310NWS")], "", 0)], State.ProcessTab[new RSTProcessKey(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("310")], "ZZ99")], 0, new DateTime(4000, 12, 31))], false, false));
            State.PhaseProcessTab.Add(new RSTPhaseProcess(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("410NWS")], "", 0)], State.ProcessTab[new RSTProcessKey(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("410")], "ZZ99")], 0, new DateTime(4000, 12, 31))], false, false));
            State.PhaseProcessTab.Add(new RSTPhaseProcess(State.IssuePhaseTab[new RSTIssuePhaseKey(State.IssueTab[new RSTIssueKey("440NWS")], "", 0)], State.ProcessTab[new RSTProcessKey(State.TemplatesTab[new RSTTemplateKey(State.WerksTab[new RSTWerksKey("440")], "ZZ99")], 0, new DateTime(4000, 12, 31))], false, false));
            // FIXME - PhaseProcess


            return State;
        }
    }
}