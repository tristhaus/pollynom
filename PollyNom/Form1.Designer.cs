namespace PollyNom
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
            this.helperPanel = new System.Windows.Forms.Panel();
            this.graphArea = new System.Windows.Forms.UserControl();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.inputBox = new System.Windows.Forms.ToolStripTextBox();
            this.calcButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helperPanel.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // helperPanel
            // 
            this.helperPanel.Controls.Add(this.graphArea);
            this.helperPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helperPanel.Location = new System.Drawing.Point(0, 27);
            this.helperPanel.Name = "helperPanel";
            this.helperPanel.Size = new System.Drawing.Size(825, 449);
            this.helperPanel.TabIndex = 0;
            // 
            // graphArea
            // 
            this.graphArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphArea.Location = new System.Drawing.Point(0, 0);
            this.graphArea.Name = "graphArea";
            this.graphArea.Size = new System.Drawing.Size(825, 449);
            this.graphArea.TabIndex = 0;
            this.graphArea.Paint += new System.Windows.Forms.PaintEventHandler(this.userControl1_Paint);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputBox,
            this.calcButton});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(825, 27);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // inputBox
            // 
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(500, 23);
            this.inputBox.Text = "x^2";
            this.inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputBox_KeyPress);
            // 
            // calcButton
            // 
            this.calcButton.Name = "calcButton";
            this.calcButton.Size = new System.Drawing.Size(45, 23);
            this.calcButton.Text = "Calc!";
            this.calcButton.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // PollyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 476);
            this.Controls.Add(this.helperPanel);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "PollyForm";
            this.Text = "Form1";
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.helperPanel.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel helperPanel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripTextBox inputBox;
        private System.Windows.Forms.ToolStripMenuItem calcButton;
        private System.Windows.Forms.UserControl graphArea;
    }
}

