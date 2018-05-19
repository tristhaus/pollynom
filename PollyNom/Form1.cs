using System;
using System.Windows.Forms;

using PollyNom.BusinessLogic;
using System.Drawing;
using System.Collections.Generic;

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

            Single x = startX;
            Single y = 0;
            Single xold = x;
            Single yold = y;
            Single scaleX = -this.Width / ((endX - startX));
            Single scaleY = -this.Height / ((endX - startX));
            Single incr = this.Width / 1000f;
            using (Pen p = new Pen(Color.Black, 2))
            {
                Graphics g = e.Graphics;

                // move coordinate system
                g.TranslateTransform(this.Width / 2, this.Height / 2);
                // draw axes
                g.DrawLine(Pens.Red, -this.Width, 0, this.Width, 0);
                g.DrawLine(Pens.Red, 0, -this.Height, 0, this.Height);
                // optimize display
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // calc function and draw it
                if (evaluator != null)
                {
                    List<PointF> points = new List<PointF>();

                    do
                    {
                        var yMaybe = evaluator.Evaluate(Convert.ToDouble(-x));

                        bool interrupt = true;
                        Single drawX = 0f;
                        Single drawY = 0f;

                        if (yMaybe.HasValue())
                        {
                            y = Convert.ToSingle(yMaybe.Value());
                            try
                            {
                                drawX = x * scaleX;
                                drawY = y * scaleY;

                                interrupt = !(-limits <= x && x <= limits && -limits <= y && y <= limits);
                            }
                            catch (System.OverflowException)
                            {
                            }
                        }

                        if (!interrupt)
                        {
                            points.Add(new PointF(drawX, drawY));
                        }
                        else
                        {
                            if (points.Count > 0)
                            {
                                g.DrawCurve(p, points.ToArray());
                                points.Clear();
                            }
                        }

                        xold = x;
                        yold = y;
                        x += incr;
                    } while (x < endX);

                    if (points.Count > 0)
                    {
                        g.DrawCurve(p, points.ToArray());
                        points.Clear();
                    }

                }
            }

            base.OnPaint(e);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            this.Refresh();
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
    }
}
