using System.Collections.Generic;

using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Dots;

namespace PollyNom.Controller
{
    /// <summary>
    /// Controls the business logic on behalf of the view.
    /// </summary>
    public class PollyController
    {
        /// <summary>
        /// Dimensions of the coordinate system in business logical units.
        /// </summary>
        private const float StartX = -11f;
        private const float EndX = 11f;
        private const float StartY = PollyController.StartX;
        private const float EndY = PollyController.EndX;
        private const float TickInterval = 1f;

        /// <summary>
        /// When exceeding this absolute limit in terms of y-value,
        /// no attempt at using the point is made
        /// </summary>
        private const float Limits = 1000f;

        /// <summary>
        /// The parser for textual representations of <see cref="IExpression"/>.
        /// </summary>
        private Parser parser;

        /// <summary>
        /// The expression currently active.
        /// </summary>
        private List<IExpression> expressions;

        /// <summary>
        /// The lists of points.
        /// </summary>
        private List<List<ListPointLogical>> points;

        /// <summary>
        /// The list of logical dots.
        /// </summary>
        private List<IDot> dots;

        /// <summary>
        /// The list of drawable dots.
        /// </summary>
        private List<IDrawDot> drawDots = null;

        /// <summary>
        /// The current score.
        /// </summary>
        private int score = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PollyController"/> class.
        /// </summary>
        /// <param name="dots">A list of dots that shall be respected by the <see cref="PollyController"/> instance.</param>
        public PollyController(List<IDot> dots = null)
        {
            this.expressions = new List<IExpression>(5);
            this.points = new List<List<ListPointLogical>>(5);
            this.dots = dots ?? new GoodDotsGenerator(8).Generate();

            this.parser = new Parser();

            this.UpdateData();
        }

        /// <summary>
        /// Gets the information as needed for the drawing of the coordinate system.
        /// </summary>
        public CoordinateSystemInfo CoordinateSystemInfo
        {
            get
            {
                return new CoordinateSystemInfo()
                {
                    StartX = PollyController.StartX,
                    EndX = PollyController.EndX,
                    StartY = PollyController.StartY,
                    EndY = PollyController.EndY,
                    TickInterval = PollyController.TickInterval,
                };
            }
        }

        /// <summary>
        /// Gets the current score.
        /// </summary>
        public int Score
        {
            get
            {
                return this.score;
            }
        }

        /// <summary>
        /// Gets the number of expressions currently handled by this controller.
        /// </summary>
        public int ExpressionCount
        {
            get
            {
                return this.expressions.Count;
            }
        }

        /// <summary>
        /// Tests the expression for parseability.
        /// </summary>
        /// <param name="textRepresentation">The textual representation of the expression.</param>
        /// <returns><c>true</c> if string is parseable.</returns>
        public bool TestExpression(string textRepresentation)
        {
            return this.parser.IsParseable(textRepresentation);
        }

        /// <summary>
        /// Updates the expression and triggers the updating of the resulting data.
        /// </summary>
        /// <param name="textRepresentation">The textual representation of the expression.</param>
        public void UpdateExpression(string textRepresentation)
        {
            if (string.IsNullOrWhiteSpace(textRepresentation))
            {
                if (this.expressions.Count > 0)
                {
                    this.expressions.RemoveAt(this.expressions.Count - 1);
                }
            }
            else
            {
                this.expressions.Add(this.parser.Parse(textRepresentation));
            }

            this.UpdateData();
        }

        /// <summary>
        /// Get the lists of plottable points in terms of business logic units.
        /// </summary>
        /// <param name="index">The index of the internally held <see cref="IExpression"/>.</param>
        /// <returns>A list of point list, in terms of business logic units.</returns>
        public List<ListPointLogical> GetListsOfLogicalPointsByIndex(int index)
        {
            if (this.points.Count > index)
            {
                return this.points[index];
            }
            else
            {
                return new List<ListPointLogical>(0);
            }
        }

       /// <summary>
        /// Get the drawable dots.
        /// </summary>
        /// <returns>A list of drawable dots.</returns>
        public List<IDrawDot> GetDrawDots()
        {
            return this.drawDots ?? new List<IDrawDot>(0);
        }

        /// <summary>
        /// Drives the updating process by delegating to other private methods.
        /// </summary>
        private void UpdateData()
        {
            this.UpdateGraphs();
            this.UpdateDotsAndScore();
        }

        private void UpdateGraphs()
        {
            this.points = new List<List<ListPointLogical>>(this.expressions.Count);
            foreach (var expression in this.expressions)
            {
                PointListGenerator pointListGenerator = new PointListGenerator(expression, PollyController.StartX, PollyController.EndX, PollyController.Limits);
                this.points.Add(pointListGenerator.ObtainListsOfLogicalPoints());
            }
        }

        private void UpdateDotsAndScore()
        {
            this.drawDots = new List<IDrawDot>(this.dots.Count);
            List<int> numbersOfHits = new List<int>(1);

            this.dots.ForEach(x => this.drawDots.Add(new DrawDot(x.Position.Item1, x.Position.Item2, x.Radius, x.GetType() == typeof(GoodDot) ? DrawDotKind.GoodDot : DrawDotKind.BadDot)));

            for (int expressionIndex = 0; expressionIndex < this.expressions.Count; ++expressionIndex)
            {
                var expression = this.expressions[expressionIndex];

                int countOfHits = 0;
                for (int dotIndex = 0; dotIndex < this.drawDots.Count; ++dotIndex)
                {
                    DrawDot drawDot = this.drawDots[dotIndex] as DrawDot;
                    if (!drawDot.IsHit)
                    {
                        bool isHit = this.dots[dotIndex].IsHit(expression, this.points[expressionIndex]);
                        if (isHit)
                        {
                            ++countOfHits;
                            drawDot.IsHit = true;
                        }
                    }
                }

                numbersOfHits.Add(countOfHits);
            }

            this.score = ScoreCalculator.CalculateScore(numbersOfHits);
        }
    }
}
