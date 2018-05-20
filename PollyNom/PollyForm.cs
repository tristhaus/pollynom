using System;
using System.Windows.Forms;

using PollyNom.BusinessLogic;
using System.Drawing;

namespace PollyNom
{
    public partial class PollyForm : Form
    {
        const float startX = 11f;
        const float endX = 11f;
        const float limits = 1000f;

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
            Int32 workingWidth = this.graphArea.Width;
            Int32 workingHeight = this.graphArea.Height;

            float scaleX = -workingWidth / ((PollyForm.endX - PollyForm.startX));
            float scaleY = -workingHeight / ((PollyForm.endX - PollyForm.startX));

            using (Pen p = new Pen(Color.Black, 2))
            {
                Graphics g = e.Graphics;

                DrawCoordinateSystem(workingWidth, workingHeight, g);

                // calc function and draw it
                if (evaluator != null)
                {
                    PointListGenerator pointListGenerator = new PointListGenerator(evaluator, PollyForm.startX, PollyForm.endX, PollyForm.limits);
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

        private void DrawCoordinateSystem(int workingWidth, int workingHeight, Graphics g)
        {
            // move coordinate system
            g.TranslateTransform(workingWidth / 2, workingHeight / 2);
            // draw axes
            g.DrawLine(Pens.Red, -workingWidth, 0, workingWidth, 0);
            g.DrawLine(Pens.Red, 0, -workingHeight, 0, workingHeight);
            // optimize display
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.graphArea.Hide();
            this.resizeClient();
            this.graphArea.Show();
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
        }

        private void PollyForm_ResizeBegin(object sender, EventArgs e)
        {
            this.graphArea.Hide();
        }

        private void PollyForm_ResizeEnd(object sender, EventArgs e)
        {
            this.graphArea.Show();
            this.Refresh();
        }
    }
}
