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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.btnMessage = new System.Windows.Forms.Button();
            this.lbRecordSet = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbTable = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox2 = new MultiSelListBox();
            this.listBox1 = new MultiSelListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "CSActiveXCtrl";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "RecordSet";
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(11, 61);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(153, 20);
            this.tbMessage.TabIndex = 3;
            this.tbMessage.Text = "Text Message";
            this.tbMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMessage_KeyDown);
            // 
            // btnMessage
            // 
            this.btnMessage.Location = new System.Drawing.Point(170, 59);
            this.btnMessage.Name = "btnMessage";
            this.btnMessage.Size = new System.Drawing.Size(131, 23);
            this.btnMessage.TabIndex = 4;
            this.btnMessage.Text = "Raise Test Event";
            this.btnMessage.UseVisualStyleBackColor = true;
            this.btnMessage.Click += new System.EventHandler(this.btnMessage_Click);
            // 
            // lbRecordSet
            // 
            this.lbRecordSet.AutoSize = true;
            this.lbRecordSet.Location = new System.Drawing.Point(86, 35);
            this.lbRecordSet.Name = "lbRecordSet";
            this.lbRecordSet.Size = new System.Drawing.Size(13, 13);
            this.lbRecordSet.TabIndex = 2;
            this.lbRecordSet.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(170, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Table";
            // 
            // lbTable
            // 
            this.lbTable.AutoSize = true;
            this.lbTable.Location = new System.Drawing.Point(259, 34);
            this.lbTable.Name = "lbTable";
            this.lbTable.Size = new System.Drawing.Size(13, 13);
            this.lbTable.TabIndex = 7;
            this.lbTable.Text = "0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(226, -1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Stream";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            //this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.splitContainer1.Location = new System.Drawing.Point(0, 251);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //this.splitContainer1.Size = new System.Drawing.Size(380, 260);
            //this.splitContainer1.SplitterDistance = 128;
            this.splitContainer1.TabIndex = 11;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // listBox2
            // 
            this.listBox2.AllowDrop = true;
            this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(3, 176);
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox2.Size = new System.Drawing.Size(376, 69);
            this.listBox2.TabIndex = 8;
            // 
            // listBox1
            // 
            this.listBox1.AllowDrop = true;
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 87);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(374, 82);
            this.listBox1.TabIndex = 5;
            // 
            // CSActiveXCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
if (false) {
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.lbTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lbRecordSet);
            this.Controls.Add(this.btnMessage);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
}
            this.Name = "CSActiveXCtrl";
            //this.Size = new System.Drawing.Size(380, 513);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button btnMessage;
        private System.Windows.Forms.Label lbRecordSet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbTable;
        private MultiSelListBox listBox1;
        private MultiSelListBox listBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
