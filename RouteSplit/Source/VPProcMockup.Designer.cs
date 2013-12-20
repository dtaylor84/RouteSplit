namespace RouteSplit
{
    partial class VPProcMockup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VPProcMockup));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvPackPhase = new RouteSplit.PackPhaseListView();
            this.chIssue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chIssueDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPhaseModel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPhaseNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWerks = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWerksName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWave = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWaveText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chActive = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lvVptgiConfig = new RouteSplit.VptgiConfigListView();
            this.chVpType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chVpTypeText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSelected = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSequence = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvProcess = new RouteSplit.ProcessListView();
            this.chId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWerksP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTemplate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTemplateText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chShipDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSeqNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chExtract = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvPackPhase);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // lvPackPhase
            // 
            this.lvPackPhase.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chIssue,
            this.chIssueDescription,
            this.chPhaseModel,
            this.chPhaseNo,
            this.chWerks,
            this.chWerksName,
            this.chWave,
            this.chWaveText,
            this.chActive});
            resources.ApplyResources(this.lvPackPhase, "lvPackPhase");
            this.lvPackPhase.HideSelection = false;
            this.lvPackPhase.Name = "lvPackPhase";
            this.lvPackPhase.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvPackPhase.UseCompatibleStateImageBehavior = false;
            this.lvPackPhase.View = System.Windows.Forms.View.Details;
            // 
            // chIssue
            // 
            resources.ApplyResources(this.chIssue, "chIssue");
            // 
            // chIssueDescription
            // 
            resources.ApplyResources(this.chIssueDescription, "chIssueDescription");
            // 
            // chPhaseModel
            // 
            resources.ApplyResources(this.chPhaseModel, "chPhaseModel");
            // 
            // chPhaseNo
            // 
            resources.ApplyResources(this.chPhaseNo, "chPhaseNo");
            // 
            // chWerks
            // 
            resources.ApplyResources(this.chWerks, "chWerks");
            // 
            // chWerksName
            // 
            resources.ApplyResources(this.chWerksName, "chWerksName");
            // 
            // chWave
            // 
            resources.ApplyResources(this.chWave, "chWave");
            // 
            // chWaveText
            // 
            resources.ApplyResources(this.chWaveText, "chWaveText");
            // 
            // chActive
            // 
            resources.ApplyResources(this.chActive, "chActive");
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvVptgiConfig);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lvProcess);
            // 
            // lvVptgiConfig
            // 
            this.lvVptgiConfig.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSequence,
            this.chVpType,
            this.chVpTypeText,
            this.chSelected});
            resources.ApplyResources(this.lvVptgiConfig, "lvVptgiConfig");
            this.lvVptgiConfig.HideSelection = false;
            this.lvVptgiConfig.Name = "lvVptgiConfig";
            this.lvVptgiConfig.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvVptgiConfig.UseCompatibleStateImageBehavior = false;
            this.lvVptgiConfig.View = System.Windows.Forms.View.Details;
            // 
            // chVpType
            // 
            resources.ApplyResources(this.chVpType, "chVpType");
            // 
            // chVpTypeText
            // 
            resources.ApplyResources(this.chVpTypeText, "chVpTypeText");
            // 
            // chSelected
            // 
            resources.ApplyResources(this.chSelected, "chSelected");
            // 
            // chSequence
            // 
            resources.ApplyResources(this.chSequence, "chSequence");
            // 
            // lvProcess
            // 
            this.lvProcess.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chId,
            this.chWerksP,
            this.chTemplate,
            this.chTemplateText,
            this.chShipDate,
            this.chSeqNo,
            this.chExtract});
            resources.ApplyResources(this.lvProcess, "lvProcess");
            this.lvProcess.HideSelection = false;
            this.lvProcess.Name = "lvProcess";
            this.lvProcess.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvProcess.UseCompatibleStateImageBehavior = false;
            this.lvProcess.View = System.Windows.Forms.View.Details;
            // 
            // chId
            // 
            resources.ApplyResources(this.chId, "chId");
            // 
            // chWerksP
            // 
            resources.ApplyResources(this.chWerksP, "chWerksP");
            // 
            // chTemplate
            // 
            resources.ApplyResources(this.chTemplate, "chTemplate");
            // 
            // chTemplateText
            // 
            resources.ApplyResources(this.chTemplateText, "chTemplateText");
            // 
            // chShipDate
            // 
            resources.ApplyResources(this.chShipDate, "chShipDate");
            // 
            // chSeqNo
            // 
            resources.ApplyResources(this.chSeqNo, "chSeqNo");
            // 
            // chExtract
            // 
            resources.ApplyResources(this.chExtract, "chExtract");
            // 
            // VPProcMockup
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "VPProcMockup";
            this.Load += new System.EventHandler(this.VPProcMockup_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private PackPhaseListView lvPackPhase;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private VptgiConfigListView lvVptgiConfig;
        private ProcessListView lvProcess;
        private System.Windows.Forms.ColumnHeader chIssue;
        private System.Windows.Forms.ColumnHeader chIssueDescription;
        private System.Windows.Forms.ColumnHeader chPhaseModel;
        private System.Windows.Forms.ColumnHeader chPhaseNo;
        private System.Windows.Forms.ColumnHeader chWerks;
        private System.Windows.Forms.ColumnHeader chWerksName;
        private System.Windows.Forms.ColumnHeader chWave;
        private System.Windows.Forms.ColumnHeader chWaveText;
        private System.Windows.Forms.ColumnHeader chActive;
        private System.Windows.Forms.ColumnHeader chVpType;
        private System.Windows.Forms.ColumnHeader chVpTypeText;
        private System.Windows.Forms.ColumnHeader chSelected;
        private System.Windows.Forms.ColumnHeader chSequence;
        private System.Windows.Forms.ColumnHeader chId;
        private System.Windows.Forms.ColumnHeader chWerksP;
        private System.Windows.Forms.ColumnHeader chTemplate;
        private System.Windows.Forms.ColumnHeader chTemplateText;
        private System.Windows.Forms.ColumnHeader chShipDate;
        private System.Windows.Forms.ColumnHeader chSeqNo;
        private System.Windows.Forms.ColumnHeader chExtract;
    }
}