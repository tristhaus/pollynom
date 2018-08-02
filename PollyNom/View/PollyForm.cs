using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using PollyNom.Controller;
using PollyNom.BusinessLogic;
using System.Linq;

namespace PollyNom.View
{
    public partial class PollyForm : Form
    {
        /// <summary>
        /// The maximum number of graphs supported by this visualizer.
        /// </summary>
        private const int maxGraphCount = 5;

        /// <summary>
        /// To be used in the title bar.
        /// </summary>
        private const string titlePrefix = "PollyNom - Score: ";

        /// <summary>
        /// Controller providing access to data and accepting commands.
        /// </summary>
        private PollyController controller;

        /// <summary>
        /// Color chosen for the coordinate system.
        /// </summary>
        private readonly Color coordinateSystemColor = Color.Red;

        /// <summary>
        /// Color chosen for the graph of the function.
        /// </summary>
        private readonly Color[] graphColors = { Color.Black, Color.Blue, Color.Green, Color.Pink, Color.Brown };

        /// <summary>
        /// Color chosen for a good dot that has not been hit.
        /// </summary>
        private readonly Color goodDotAsleepColor = Color.LightBlue;

        /// <summary>
        /// Color chosen for a good dot that has been hit.
        /// </summary>
        private readonly Color goodDotHitColor = Color.DarkBlue;

        /// <summary>
        /// Creates a new instance of the <see cref="PollyForm"/> class.
        /// </summary>
        public PollyForm()
        {
            this.controller = new PollyController();

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
            this.ReadAndDelegate();
            this.Refresh();
        }

        /// <summary>
        /// Handles the change of the input text box by evaluating
        /// whether the text is parseable and changing the text color
        /// accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputBox_TextChanged(object sender, EventArgs e)
        {
            bool isParseable = this.controller.TestExpression(inputBox.Text);
            var oldColor = inputBox.ForeColor;
            inputBox.ForeColor = isParseable ? SystemColors.WindowText : Color.Red;
            var newColor = inputBox.ForeColor;
            if (!oldColor.Equals(newColor))
            {
                this.Refresh();
            }
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
            if (e.KeyChar == (Char)Keys.Enter && (this.controller.TestExpression(this.inputBox.Text) || string.IsNullOrWhiteSpace(this.inputBox.Text)))
            {
                this.ReadAndDelegate();
                this.Refresh();
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Reads the user input text and parses the expression from it.
        /// </summary>
        private void ReadAndDelegate()
        {
            if ((!string.IsNullOrWhiteSpace(inputBox.Text) && this.controller.ExpressionCount < 5) || string.IsNullOrWhiteSpace(inputBox.Text))
            {
                this.controller.UpdateExpression(inputBox.Text);
            }
        }

        /// <summary>
        /// Handles the refreshing of the coordinate system and contents.
        /// </summary>
        /// <param name="sender">?</param>
        /// <param name="e">?</param>
        private void userControl1_Paint(object sender, PaintEventArgs e)
        {
            DrawContents(e);

            base.OnPaint(e);
        }

        private void DrawContents(PaintEventArgs e)
        {
            Int32 workingWidth = this.graphArea.Width;
            Int32 workingHeight = this.graphArea.Height;

            CoordinateSystemInfo coordinateSystemInfo = controller.CoordinateSystemInfo;

            float scaleX = -workingWidth / ((coordinateSystemInfo.StartX - coordinateSystemInfo.EndX));
            float scaleY = -workingHeight / ((coordinateSystemInfo.StartY - coordinateSystemInfo.EndY));

            Graphics g = e.Graphics;

            this.UpdateWindowTitle();

            this.DrawCoordinateSystem(g, workingWidth, workingHeight);

            this.DrawGraphs(g, scaleX, scaleY);

            this.DrawDots(g, scaleX, scaleY);
        }

        /// <summary>
        /// Updates the window title with the current score.
        /// </summary>
        private void UpdateWindowTitle()
        {
            this.Text = PollyForm.titlePrefix + $"{this.controller.Score}";
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
                CoordinateSystemInfo coordinateSystemInfo = controller.CoordinateSystemInfo;

                // move coordinate system
                g.TranslateTransform(theWidth / 2, theHeight / 2);

                // draw axes
                g.DrawLine(coordinatePen, -theWidth, 0, theWidth, 0);
                g.DrawLine(coordinatePen, 0, -theHeight, 0, theHeight);

                // draw ticks
                const float proportionTick = 1f / 85f;
                float tickSingleHeight = theHeight * proportionTick;
                float tickSingleWidth = theWidth * proportionTick;
                float horizontalUnit = Math.Abs(theWidth / (coordinateSystemInfo.StartX - coordinateSystemInfo.EndX));
                float verticalUnit = -Math.Abs(theHeight / (coordinateSystemInfo.StartY - coordinateSystemInfo.EndY));
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
        /// Draw the graph on the <see cref="Graphics"/> <paramref name="g"/>.
        /// </summary>
        /// <param name="g">The graphics to be drawn on.</param>
        /// <param name="scaleX">Horizontal scaling factor.</param>
        /// <param name="scaleY">Vertical scaling factor.</param>
        private void DrawGraphs(Graphics g, float scaleX, float scaleY)
        {
            for (int index = 0; index < this.controller.ExpressionCount; index++)
            {
                List<ListPointLogical> logicalPointLists = this.controller.GetListsOfLogicalPointsByIndex(index);
                List<List<PointF>> pointLists = PollyFormHelper.ConvertToScaledPoints(logicalPointLists, scaleX, -scaleY);

                using (Pen graphPen = new Pen(this.graphColors[index], 2))
                {
                    pointLists
                        .FindAll(pointList => pointList.Count > 1)
                        .ForEach(nonEmptyPointList => g.DrawCurve(graphPen, nonEmptyPointList.ToArray()));
                }
            }
        }

        /// <summary>
        /// Draw the dots on the <see cref="Graphics"/> <paramref name="g"/>.
        /// </summary>
        /// <param name="g">The graphics to be drawn on.</param>
        /// <param name="scaleX">Horizontal scaling factor.</param>
        /// <param name="scaleY">Vertical scaling factor.</param>
        private void DrawDots(Graphics g, float scaleX, float scaleY)
        {
            using (Brush goodDotAsleepBrush = new SolidBrush(this.goodDotAsleepColor))
            using (Brush goodDotHitBrush = new SolidBrush(this.goodDotHitColor))
            {
                foreach (var goodDot in this.controller.GetDrawDots())
                {
                    g.FillEllipse(
                        goodDot.IsHit ? goodDotHitBrush : goodDotAsleepBrush,
                        (float)(goodDot.Position.Item1 - goodDot.Radius) * (scaleX),
                        (float)(goodDot.Position.Item2 + goodDot.Radius) * (-scaleY),
                        (float)(2.0f * goodDot.Radius) * Math.Abs(scaleY),
                        (float)(2.0f * goodDot.Radius) * Math.Abs(scaleX));
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
            this.graphPanel.Hide();
            this.inputPanel.Hide();
            this.resizeClients();
            this.graphPanel.Show();
            this.inputPanel.Show();
        }

        /// <summary>
        /// Handles the begin of a resizing on <see cref="PollyForm"/>
        /// by hiding the coordinate system.
        /// </summary>
        /// <param name="sender">?</param>
        /// <param name="e">?</param>
        private void PollyForm_ResizeBegin(object sender, EventArgs e)
        {
            this.graphPanel.Hide();
            this.inputPanel.Hide();
        }

        /// <summary>
        /// Handles the end of a resizing on <see cref="PollyForm"/>
        /// by updating and showing the coordinate system.
        /// </summary>
        /// <param name="sender">?</param>
        /// <param name="e">?</param>
        private void PollyForm_ResizeEnd(object sender, EventArgs e)
        {
            this.graphPanel.Show();
            this.inputPanel.Show();
            this.Refresh();
        }

        /// <summary>
        /// Resizes the graph area according to <see cref="PollyForm"/> dimensions.
        /// </summary>
        private void resizeClients()
        {
            // hail mary and resize
            var rect = this.ClientRectangle;
            var verticalOffset = this.inputPanel.Height;
            rect.Size = new Size(rect.Width, rect.Height - verticalOffset);

            if (rect.Height >= rect.Width)
            {
                this.graphPanel.Size = new Size(rect.Width, rect.Width);
                var toDistribute = rect.Height - rect.Width;
                this.graphPanel.Left = 0;
                this.graphPanel.Top = toDistribute / 2;
            }
            else
            {
                this.graphPanel.Size = new Size(rect.Height, rect.Height);
                var toDistribute = rect.Width - rect.Height;
                this.graphPanel.Left = toDistribute / 2;
                this.graphPanel.Top = 0;
            }

            // resize the input box
            int currWidth = this.inputBox.Width;
            int x = toolStrip1.Items.OfType<ToolStripItem>().Sum(t => t.Width);
            this.inputBox.Size = new Size(toolStrip1.Width - x + currWidth - 12, this.inputBox.Height);
        }
    }
}
