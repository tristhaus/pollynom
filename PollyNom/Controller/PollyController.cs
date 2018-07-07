using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Dots;
using PollyNom.BusinessLogic.Expressions;

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

        private List<IDrawDot> drawDots = null;
        private List<IDot> dots;

        public PollyController()
        {
            this.parser = new Parser();
            this.dots = new GoodDotsGenerator().Generate();

            this.UpdateData();
        }

        public void UpdateExpression(string textRepresentation)
        {
            if (!string.IsNullOrWhiteSpace(textRepresentation))
            {
                this.expression = new Parser().Parse(textRepresentation);
                this.UpdateData();
            }
        }

        public IExpression PROVISIONAL_GetExpression()
        {
            return this.expression ?? new InvalidExpression();
        }

        public List<ListPointLogical> PROVISIONAL_GetListsOfLogicalPoints()
        {
            return points ?? new List<ListPointLogical>(0);
        }

        public List<IDrawDot> GetDrawDots()
        {
            return drawDots ?? new List<IDrawDot>(0);
        }

        private void UpdateData()
        {
            if (this.expression != null)
            {
                PointListGenerator pointListGenerator = new PointListGenerator(this.expression, PollyController.startX, PollyController.endX, PollyController.limits);
                this.points = pointListGenerator.ObtainListsOfLogicalsPoints();
            }

            UpdateDots();
        }

        private void UpdateDots()
        {
            this.drawDots = new List<IDrawDot>(this.dots.Count);
            foreach (var dot in this.dots)
            {
                bool isHit = dot.IsHit(this.expression ?? new BusinessLogic.Expressions.InvalidExpression(), this.points);
                DrawDotKind kind = dot.GetType() == typeof(GoodDot) ? DrawDotKind.GoodDot : DrawDotKind.BadDot;
                drawDots.Add(new DrawDot(dot.Position.Item1, dot.Position.Item2, dot.Radius, isHit, kind));
            }
        }
    }
}
