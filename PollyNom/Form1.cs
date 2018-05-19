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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputBox.Text))
            {
                return;
            }
            this.evaluator = new Evaluator(inputBox.Text);

            this.Refresh();
        }

        private void userControl1_Paint(object sender, PaintEventArgs e)
        {
            Single x = -40;
            Single y = 0;
            Single xold = x;
            Single yold = y;
            Single scalex = this.Width / (2 * x);
            Single scaley = -(this.Height - 20) / 2;
            Single incr = 80f / this.Width;
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
                if(evaluator != null)
                {
                    for (int i = 0; i < this.Width; i++)
                    {
                        var yMaybe = evaluator.Evaluate(Convert.ToDouble(x));

                        x += incr;
                        if (yMaybe.HasValue()) {
                            y = Convert.ToSingle(yMaybe.Value());
                            g.DrawLine(p, x * scalex, y * scaley, xold * scalex, yold * scaley);
                        }

                        xold = x;
                        yold = y;
                    }
                }
            }

            base.OnPaint(e);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
