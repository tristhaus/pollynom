using System;
using System.Windows.Forms;

using PollyNom.BusinessLogic;
using System.Drawing;

namespace PollyNom
{
    public partial class PollyForm : Form
    {
        Evaluator evaluator;

        public PollyForm()
        {
            this.ResizeRedraw = true;
            InitializeComponent();
        }
        
        private void RunReadAndEvaluate()
        {
            if (string.IsNullOrWhiteSpace(inputBox.Text))
            {
                return;
            }
            this.evaluator = new Evaluator(inputBox.Text);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.RunReadAndEvaluate();
            this.Refresh();
        }

        private void userControl1_Paint(object sender, PaintEventArgs e)
        {
            Single startX = -10f;
            Single endX = 10f;
            Single limits = 1000f;

            Int32 workingWidth = this.graphArea.Width;
            Int32 workingHeight = this.graphArea.Height;

            Single scaleX = -workingWidth / ((endX - startX));
            Single scaleY = -workingHeight / ((endX - startX));

            using (Pen p = new Pen(Color.Black, 2))
            {
                Graphics g = e.Graphics;

                // move coordinate system
                g.TranslateTransform(workingWidth / 2, workingHeight / 2);
                // draw axes
                g.DrawLine(Pens.Red, -workingWidth, 0, workingWidth, 0);
                g.DrawLine(Pens.Red, 0, -workingHeight, 0, workingHeight);
                // optimize display
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // calc function and draw it
                if (evaluator != null)
                {
                    PointListGenerator pointListGenerator = new PointListGenerator(evaluator, startX, endX, limits);
                    var pointLists = pointListGenerator.ObtainScaledPoints(-scaleX, scaleY);

                    foreach (var pointList in pointLists)
                    {
                        if (pointList.Count > 1)
                        {
                            g.DrawCurve(p, pointList.ToArray());
                        }
                    }
                }
            }

            base.OnPaint(e);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.resizeClient();
        }

        private void inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                this.RunReadAndEvaluate();
                this.Refresh();
                e.Handled = true;
            }
        }

        private void resizeClient()
        {
            // hail mary and resize
            var rect = this.ClientRectangle;
            var verticalOffset = this.menuStrip.Height;
            rect.Size = new Size(rect.Width, rect.Height - verticalOffset);

            if (rect.Height >= rect.Width)
            {
                this.graphArea.Size = new Size(rect.Width, rect.Width);
                var toDistribute = rect.Height - rect.Width;
                this.graphArea.Left = 0;
                this.graphArea.Top = verticalOffset + toDistribute / 2;
            }
            else
            {
                this.graphArea.Size = new Size(rect.Height, rect.Height);
                var toDistribute = rect.Width - rect.Height;
                this.graphArea.Left = toDistribute / 2;
                this.graphArea.Top = verticalOffset;
            }

            this.Refresh();
        }
    }
}
