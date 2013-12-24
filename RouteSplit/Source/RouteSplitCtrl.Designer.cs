namespace RouteSplit
{
    partial class RouteSplitCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.vPProcMockupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rSCtrlFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vpProcMockup1 = new RouteSplit.VPProcMockup();
            this.rsCtrlForm1 = new RouteSplit.RSCtrlForm();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vPProcMockupToolStripMenuItem,
            this.rSCtrlFormToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(856, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // vPProcMockupToolStripMenuItem
            // 
            this.vPProcMockupToolStripMenuItem.Name = "vPProcMockupToolStripMenuItem";
            this.vPProcMockupToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.vPProcMockupToolStripMenuItem.Text = "VPProcMockup";
            this.vPProcMockupToolStripMenuItem.Click += new System.EventHandler(this.vPProcMockupToolStripMenuItem_Click);
            // 
            // rSCtrlFormToolStripMenuItem
            // 
            this.rSCtrlFormToolStripMenuItem.Name = "rSCtrlFormToolStripMenuItem";
            this.rSCtrlFormToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.rSCtrlFormToolStripMenuItem.Text = "RSCtrlForm";
            this.rSCtrlFormToolStripMenuItem.Click += new System.EventHandler(this.rSCtrlFormToolStripMenuItem_Click);
            // 
            // vpProcMockup1
            // 
            this.vpProcMockup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vpProcMockup1.Location = new System.Drawing.Point(0, 24);
            this.vpProcMockup1.Name = "vpProcMockup1";
            this.vpProcMockup1.Size = new System.Drawing.Size(856, 489);
            this.vpProcMockup1.TabIndex = 0;
            // 
            // rsCtrlForm1
            // 
            this.rsCtrlForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rsCtrlForm1.Location = new System.Drawing.Point(0, 24);
            this.rsCtrlForm1.Name = "rsCtrlForm1";
            this.rsCtrlForm1.Size = new System.Drawing.Size(856, 489);
            this.rsCtrlForm1.State = null;
            this.rsCtrlForm1.TabIndex = 1;
            this.rsCtrlForm1.Visible = false;
            // 
            // RouteSplitCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vpProcMockup1);
            this.Controls.Add(this.rsCtrlForm1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "RouteSplitCtrl";
            this.Size = new System.Drawing.Size(856, 513);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VPProcMockup vpProcMockup1;
        private RSCtrlForm rsCtrlForm1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem vPProcMockupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rSCtrlFormToolStripMenuItem;
    }
}
