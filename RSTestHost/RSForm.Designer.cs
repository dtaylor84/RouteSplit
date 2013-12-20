namespace RSTestHost
{
    partial class RSForm
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
            this.rs = new RouteSplit.RouteSplitCtrl();
            this.SuspendLayout();
            // 
            // rs
            // 
            this.rs.Location = new System.Drawing.Point(0, 0);
            this.rs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rs.Name = "rs";
            this.rs.TabIndex = 0;
            // 
            // RSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1241, 589);
            this.Controls.Add(this.rs);
            this.Name = "RSForm";
            this.Text = "RouteSplit Test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private RouteSplit.RouteSplitCtrl rs;
    }
}

