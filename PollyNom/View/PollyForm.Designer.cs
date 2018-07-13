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
            this.graphArea = new System.Windows.Forms.UserControl();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.inputBox = new System.Windows.Forms.ToolStripTextBox();
            this.calcButton = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphArea
            // 
            this.graphArea.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.graphArea.Location = new System.Drawing.Point(0, 30);
            this.graphArea.Name = "graphArea";
            this.graphArea.Size = new System.Drawing.Size(559, 559);
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
            this.menuStrip.Size = new System.Drawing.Size(559, 27);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // inputBox
            // 
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(500, 23);
            this.inputBox.Text = "8*((x^2)-(x^4))";
            this.inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputBox_KeyPress);
            this.inputBox.TextChanged += new System.EventHandler(this.inputBox_TextChanged);
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
            this.ClientSize = new System.Drawing.Size(559, 586);
            this.Controls.Add(this.graphArea);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(575, 625);
            this.Name = "PollyForm";
            this.Text = "PollyNom - Score: 0";
            this.ResizeBegin += new System.EventHandler(this.PollyForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.PollyForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripTextBox inputBox;
        private System.Windows.Forms.ToolStripMenuItem calcButton;
        private System.Windows.Forms.UserControl graphArea;
    }
}

