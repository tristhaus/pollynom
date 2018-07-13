using System.Collections.Generic;

using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Dots;

namespace PollyNom.Controller
{
    public class PollyController
    {
        /// <summary>
        /// Dimensions of the coordinate system in business logical units.
        /// </summary>
        private const float startX = -11f;
        private const float endX = 11f;
        private const float startY = PollyController.startX;
        private const float endY = PollyController.endX;
        private const float TickInterval = 1f;

        /// <summary>
        /// When exceeding this absolute limit in terms of y-value, 
        /// no attempt at using the point is made
        /// </summary>
        private const float limits = 1000f;

        /// <summary>
        /// The parser for textual representations of <see cref="IExpression"/>.
        /// </summary>
        private Parser parser;

        /// <summary>
        /// The expression currently active.
        /// </summary>
        private IExpression expression;

        /// <summary>
        /// The lists of points.
        /// </summary>
        private List<ListPointLogical> points = null;

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
        /// Creates a new instance of the <see cref="PollyController"/> class.
        /// </summary>
        public PollyController()
        {
            this.parser = new Parser();
            this.dots = new GoodDotsGenerator().Generate();

            this.UpdateData();
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
            this.expression = this.parser.Parse(textRepresentation);
            this.UpdateData();
        }

        /// <summary>
        /// Get the information as needed for the drawing of the coordinate system.
        /// </summary>
        public CoordinateSystemInfo CoordinateSystemInfo
        {
            get
            {
                return new CoordinateSystemInfo()
                {
                    StartX = PollyController.startX,
                    EndX = PollyController.endX,
                    StartY = PollyController.startY,
                    EndY = PollyController.endY,
                    TickInterval = PollyController.TickInterval,
                };
            }
        }

        /// <summary>
        /// Get the lists of plottable points in terms of business logic units.
        /// </summary>
        /// <returns>A list of point list, in terms of business logic units.</returns>
        public List<ListPointLogical> GetListsOfLogicalPoints()
        {
            return points ?? new List<ListPointLogical>(0);
        }

        /// <summary>
        /// Get the drawable dots.
        /// </summary>
        /// <returns>A list of drawable dots.</returns>
        public List<IDrawDot> GetDrawDots()
        {
            return drawDots ?? new List<IDrawDot>(0);
        }

        /// <summary>
        /// Gets the current score.
        /// </summary>
        public int Score
        {
            get
            {
                return score;
            }
        }

        /// <summary>
        /// Drives the updating process by delegating to other private methods.
        /// </summary>
        private void UpdateData()
        {
            UpdateGraph();
            UpdateDotsAndScore();
        }

        private void UpdateGraph()
        {
            if (this.expression != null)
            {
                PointListGenerator pointListGenerator = new PointListGenerator(this.expression, PollyController.startX, PollyController.endX, PollyController.limits);
                this.points = pointListGenerator.ObtainListsOfLogicalsPoints();
            }
        }

        private void UpdateDotsAndScore()
        {
            this.drawDots = new List<IDrawDot>(this.dots.Count);
            List<int> numbersOfHits = new List<int>(1);

            int countOfHits = 0;
            foreach (var dot in this.dots)
            {
                bool isHit = dot.IsHit(this.expression ?? new BusinessLogic.Expressions.InvalidExpression(), this.points);
                DrawDotKind kind = dot.GetType() == typeof(GoodDot) ? DrawDotKind.GoodDot : DrawDotKind.BadDot;
                drawDots.Add(new DrawDot(dot.Position.Item1, dot.Position.Item2, dot.Radius, isHit, kind));

                if(isHit)
                {
                    ++countOfHits;
                }
            }

            numbersOfHits.Add(countOfHits);

            this.score = ScoreCalculator.CalculateScore(numbersOfHits);
        }
    }
}
