namespace PollyNom.View
{
    partial class PollyForm
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
            this.graphPanel = new System.Windows.Forms.Panel();
            this.graphArea = new System.Windows.Forms.UserControl();
            this.inputPanel = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.inputBox = new System.Windows.Forms.ToolStripTextBox();
            this.calcButton = new System.Windows.Forms.ToolStripButton();
            this.graphPanel.SuspendLayout();
            this.inputPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphPanel
            // 
            this.graphPanel.Controls.Add(this.graphArea);
            this.graphPanel.Location = new System.Drawing.Point(12, 12);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size(535, 535);
            this.graphPanel.TabIndex = 2;
            // 
            // graphArea
            // 
            this.graphArea.AutoSize = true;
            this.graphArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphArea.Location = new System.Drawing.Point(0, 0);
            this.graphArea.Name = "graphArea";
            this.graphArea.Size = new System.Drawing.Size(535, 535);
            this.graphArea.TabIndex = 0;
            this.graphArea.Paint += new System.Windows.Forms.PaintEventHandler(this.userControl1_Paint);
            // 
            // inputPanel
            // 
            this.inputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputPanel.AutoSize = true;
            this.inputPanel.Controls.Add(this.toolStrip1);
            this.inputPanel.Location = new System.Drawing.Point(12, 549);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Size = new System.Drawing.Size(535, 29);
            this.inputPanel.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AllowMerge = false;
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputBox,
            this.calcButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(535, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // inputBox
            // 
            this.inputBox.MaxLength = 256;
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(478, 25);
            this.inputBox.Text = "x";
            this.inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputBox_KeyPress);
            this.inputBox.TextChanged += new System.EventHandler(this.inputBox_TextChanged);
            // 
            // calcButton
            // 
            this.calcButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.calcButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.calcButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.calcButton.Name = "calcButton";
            this.calcButton.Size = new System.Drawing.Size(37, 22);
            this.calcButton.Text = "Calc!";
            this.calcButton.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // PollyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 590);
            this.Controls.Add(this.inputPanel);
            this.Controls.Add(this.graphPanel);
            this.MinimumSize = new System.Drawing.Size(575, 629);
            this.Name = "PollyForm";
            this.Text = "PollyNom - Score: 0";
            this.ResizeBegin += new System.EventHandler(this.PollyForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.PollyForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.graphPanel.ResumeLayout(false);
            this.graphPanel.PerformLayout();
            this.inputPanel.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel graphPanel;
        private System.Windows.Forms.Panel inputPanel;
        private System.Windows.Forms.ToolStripTextBox inputBox;
        private System.Windows.Forms.ToolStripButton calcButton;
        private System.Windows.Forms.UserControl graphArea;
        private System.Windows.Forms.ToolStrip toolStrip1;
    }
}

