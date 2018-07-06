using System;
using System.Collections.Generic;
using System.Windows.Forms;

using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Dots;
using System.Drawing;

namespace PollyNom
{
    public partial class PollyForm : Form
    {
        /// <summary>
        /// Dimensions of the coordinate system in business logical units.
        /// </summary>
        private const float startX = -11f;
        private const float endX = 11f;
        private const float startY = PollyForm.startX;
        private const float endY = PollyForm.endX;

        /// <summary>
        /// Color chosen for the coordinate system.
        /// </summary>
        private readonly Color coordinateSystemColor = Color.Red;

        /// <summary>
        /// Color chosen for the graph of the function.
        /// </summary>
        private readonly Color graphColor = Color.Black;

        /// <summary>
        /// Color chosen for a good dot that has not been hit.
        /// </summary>
        private readonly Color goodDotAsleepColor = Color.LightBlue;

        /// <summary>
        /// Color chosen for a good dot that has been hit.
        /// </summary>
        private readonly Color goodDotHitColor = Color.DarkBlue;
        
        /// <summary>
        /// When exceeding this absolute limit in terms of y-value, 
        /// no attempt at using the point is made
        /// </summary>
        private const float limits = 1000f;

        /// <summary>
        /// The expression currently active.
        /// </summary>
        private IExpression expression;

        /// <summary>
        /// A provisional list of good dots.
        /// </summary>
        private List<GoodDot> goodDots;

        /// <summary>
        /// Creates a new instance of the <see cref="PollyForm"/> class.
        /// </summary>
        public PollyForm()
        {
            GoodDotsGenerator generator = new GoodDotsGenerator();
            this.goodDots = generator.Generate();

            this.ResizeRedraw = true;
            InitializeComponent();
        }

        /// <summary>
        /// Handle a click on the <c>Calc!</c> button.
        /// </summary>
        /// <param name="sender">Sender, i.e. button.</param>
        /// <param name="e">EventArgs</param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.RunReadAndEvaluate();
            this.Refresh();
        }

        /// <summary>
        /// Reads the user input text and sets the appropriate evaluator.
        /// </summary>
        private void RunReadAndEvaluate()
        {
            if (string.IsNullOrWhiteSpace(inputBox.Text))
            {
                return;
            }
            this.expression = new Parser().Parse(inputBox.Text);
        }

        /// <summary>
        /// Handles the refreshing of the coordinate system and contents.
        /// </summary>
        /// <param name="sender">?</param>
        /// <param name="e">?</param>
        private void userControl1_Paint(object sender, PaintEventArgs e)
        {
            Int32 workingWidth = this.graphArea.Width;
            Int32 workingHeight = this.graphArea.Height;

            float scaleX = -workingWidth / ((PollyForm.startX - PollyForm.endX));
            float scaleY = -workingHeight / ((PollyForm.startY - PollyForm.endY));

            Graphics g = e.Graphics;

            this.DrawCoordinateSystem(g, workingWidth, workingHeight);

            List<ListPointLogical> logicalPointLists = null;

            // calc function and draw it
            if (this.expression != null)
            {
                PointListGenerator pointListGenerator = new PointListGenerator(this.expression, PollyForm.startX, PollyForm.endX, PollyForm.limits);
                logicalPointLists = pointListGenerator.ObtainListsOfLogicalsPoints();
                List<List<PointF>> pointLists = pointListGenerator.ConvertToScaledPoints(logicalPointLists, scaleX, -scaleY);

                using (Pen graphPen = new Pen(this.graphColor, 2))
                {
                    pointLists
                        .FindAll(pointList => pointList.Count > 1)
                        .ForEach(nonEmptyPointList => g.DrawCurve(graphPen, nonEmptyPointList.ToArray()));
                }
            }

            this.DrawDots(g, logicalPointLists, scaleX, scaleY);

            base.OnPaint(e);
        }

        /// <summary>
        /// Draw a coordinate system on the <see cref="Graphics"/> <paramref name="g"/>.
        /// </summary>
        /// <param name="g">The graphics to be drawn on.</param>
        /// <param name="theWidth">The width to be used for the drawing.</param>
        /// <param name="theHeight">The height to be used for the drawing.</param>
        private void DrawCoordinateSystem(Graphics g, int theWidth, int theHeight)
        {
            using (Pen coordinatePen = new Pen(this.coordinateSystemColor))
            using (Brush coordinateBrush = new SolidBrush(this.coordinateSystemColor))
            using (FontFamily segoeUIFamily = new FontFamily("Segoe UI"))
            using (Font segoeUIFont = new Font(segoeUIFamily, 9.0f)) // Segoe UI; 9pt
            {
                // move coordinate system
                g.TranslateTransform(theWidth / 2, theHeight / 2);

                // draw axes
                g.DrawLine(coordinatePen, -theWidth, 0, theWidth, 0);
                g.DrawLine(coordinatePen, 0, -theHeight, 0, theHeight);

                // draw ticks
                const float proportionTick = 1f / 85f;
                float tickSingleHeight = theHeight * proportionTick;
                float tickSingleWidth = theWidth * proportionTick;
                float horizontalUnit = Math.Abs(theWidth / (PollyForm.startX - PollyForm.endX));
                float verticalUnit = -Math.Abs(theHeight / (PollyForm.startY - PollyForm.endY));
                for (int i = 1; i < 11; i++)
                {
                    float localFactor = 1.0f;
                    if (i % 10 == 0)
                    {
                        localFactor = 1.40f;
                    }
                    else if (i % 5 == 0)
                    {
                        localFactor = 1.20f;
                    }

                    g.DrawLine(coordinatePen, +i * horizontalUnit, localFactor * tickSingleHeight, +i * horizontalUnit, -localFactor * tickSingleHeight);
                    g.DrawLine(coordinatePen, -i * horizontalUnit, localFactor * tickSingleHeight, -i * horizontalUnit, -localFactor * tickSingleHeight);
                    g.DrawLine(coordinatePen, localFactor * tickSingleWidth, +i * verticalUnit, -localFactor * tickSingleWidth, +i * verticalUnit);
                    g.DrawLine(coordinatePen, localFactor * tickSingleWidth, -i * verticalUnit, -localFactor * tickSingleWidth, -i * verticalUnit);
                }

                // draw arrowheads
                PointF[] xArrowPoints = { new PointF(10.4f * horizontalUnit, tickSingleHeight * 1.20f), new PointF(10.4f * horizontalUnit, -tickSingleHeight * 1.20f), new PointF(10.95f * horizontalUnit, 0.0f) };
                PointF[] yArrowPoints = { new PointF(tickSingleHeight * 1.20f, 10.4f * verticalUnit), new PointF(-tickSingleWidth * 1.20f, 10.4f * verticalUnit), new PointF(0.0f, 10.95f * verticalUnit) };
                g.FillPolygon(coordinateBrush, xArrowPoints);
                g.FillPolygon(coordinateBrush, yArrowPoints);

                // add labels
                g.DrawString("-10", segoeUIFont, coordinateBrush, new PointF(-10.0f * horizontalUnit, tickSingleHeight * 1.5f));
                g.DrawString("10", segoeUIFont, coordinateBrush, new PointF(10.0f * horizontalUnit, tickSingleHeight * 1.5f));
                g.DrawString("-10", segoeUIFont, coordinateBrush, new PointF(tickSingleWidth * 1.5f, -10.0f * verticalUnit));
                g.DrawString("10", segoeUIFont, coordinateBrush, new PointF(tickSingleWidth * 1.5f, 10.0f * verticalUnit));

                // optimize display
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }
        }

        /// <summary>
        /// Draw the dots on the <see cref="Graphics"/> <paramref name="g"/>.
        /// </summary>
        /// <param name="g">The graphics to be drawn on.</param>
        /// <param name="scaleX">Horizontal scaling factor.</param>
        /// <param name="scaleY">Vertical scaling factor.</param>
        private void DrawDots(Graphics g, List<ListPointLogical> logicalPointLists, float scaleX, float scaleY)
        {
            using (Brush goodDotAsleepBrush = new SolidBrush(this.goodDotAsleepColor))
            using (Brush goodDotHitBrush = new SolidBrush(this.goodDotHitColor))
            {
                foreach (var goodDot in this.goodDots)
                {
                    g.FillEllipse(
                        goodDot.IsHit(this.expression ?? new BusinessLogic.Expressions.InvalidExpression(), logicalPointLists) ? goodDotHitBrush : goodDotAsleepBrush, 
                        (float)(goodDot.Position.Item1 - goodDot.Radius) * (scaleX),
                        (float)(goodDot.Position.Item2 + goodDot.Radius) * (-scaleY),
                        (float)(2.0f * goodDot.Radius) * Math.Abs(scaleY), 
                        (float)(2.0f * goodDot.Radius) * Math.Abs(scaleX)
                        );
                }
            }
        }

        /// <summary>
        /// Handles the resizing of the entire main form 
        /// by disabling the coordinate system, resizing it, and
        /// reenabling it.
        /// </summary>
        /// <param name="sender">?</param>
        /// <param name="e">?</param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.graphArea.Hide();
            this.resizeClient();
            this.graphArea.Show();
        }

        /// <summary>
        /// Handles a key press in the input text box
        /// by filtering out the enter key, which simulates
        /// a button press on <c>Calc!</c>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                this.RunReadAndEvaluate();
                this.Refresh();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Resizes the graph area according to <see cref="PollyForm"/> dimensions.
        /// </summary>
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

        /// <summary>
        /// Handles the begin of a resizing on <see cref="PollyForm"/>
        /// by hiding the coordinate system.
        /// </summary>
        /// <param name="sender">?</param>
        /// <param name="e">?</param>
        private void PollyForm_ResizeBegin(object sender, EventArgs e)
        {
            this.graphArea.Hide();
        }

        /// <summary>
        /// Handles the end of a resizing on <see cref="PollyForm"/>
        /// by updating and showing the coordinate system.
        /// </summary>
        /// <param name="sender">?</param>
        /// <param name="e">?</param>
        private void PollyForm_ResizeEnd(object sender, EventArgs e)
        {
            this.graphArea.Show();
            this.Refresh();
        }
    }
}
