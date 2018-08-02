﻿using System;
using System.Collections.Generic;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// A class that generates lists of points based on a expression
    /// using intervals.
    /// </summary>
    public class PointListGenerator
    {
        /// <summary>
        /// The lower limit for increments etc.
        /// </summary>
        private const double epsilon = 1e-4;

        /// <summary>
        /// The x increments should be below this value, but above <see cref="PointListGenerator.epsilon"/>.
        /// </summary>
        private const double targetDistance = 5e-3;

        /// <summary>
        /// The starting increment for x.
        /// </summary>
        private const double initialIncrement = 1e-3;

        /// <summary>
        /// The large increment for detecting intervals.
        /// </summary>
        private const double largeIncrement = 1e-2;

        /// <summary>
        /// The expression to be evaluated.
        /// </summary>
        private IExpression expression;

        /// <summary>
        /// The inputted start of x.
        /// </summary>
        private double initialX;

        /// <summary>
        /// The inputted end of x.
        /// </summary>
        private double finalX;

        /// <summary>
        /// The inputted limits.
        /// </summary>
        private double limits;

        /// <summary>
        /// Creates a new instance of the <see cref="PointListGenerator"/> class,
        /// using an <see cref="IExpression"/> and parameters for the evaluation.
        /// </summary>
        /// <param name="expression">The expression to be used.</param>
        /// <param name="initialX">The x at which the list shall begin.</param>
        /// <param name="finalX">The x at which the list shall end.</param>
        /// <param name="limits">The absolute maximum y-value to be considered.</param>
        public PointListGenerator(IExpression expression, double initialX, double finalX, double limits)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (initialX >= finalX)
            {
                throw new ArgumentException($"{nameof(initialX)} must be smaller than {nameof(finalX)}");
            }

            this.expression = expression;
            this.initialX = initialX;
            this.finalX = finalX;
            this.limits = limits;
        }

        /// <summary>
        /// Provides access to sorted lists of points calculated from the expression
        /// using the member parameters in logical, business units.
        /// </summary>
        /// <returns>A list of sorted lists of points.</returns>
        public List<ListPointLogical> ObtainListsOfLogicalPoints()
        {
            List<ListPointLogical> retList = new List<ListPointLogical>();

            double lastXinPreviousInterval = this.initialX;
            double x = lastXinPreviousInterval;
            Maybe<double> yMaybe;

            while (x < this.finalX)
            {
                ListPointLogical points = new ListPointLogical();

                // look for interval
                double XinCurrentInterval = lastXinPreviousInterval;
                bool foundInterval = false;
                while (!foundInterval && XinCurrentInterval < this.finalX)
                {
                    XinCurrentInterval += PointListGenerator.largeIncrement;
                    yMaybe = this.expression.Evaluate(XinCurrentInterval);
                    if (yMaybe.HasValue)
                    {
                        points.Add(new PointLogical(XinCurrentInterval, yMaybe.Value));
                        foundInterval = true;
                    }
                }

                // maybe the function is not defined anywhere in our window
                if (!foundInterval)
                {
                    break;
                }

                double lastXinCurrentInterval;

                // work inside interval - backward
                this.workAnInterval(d => -d, out x, ref points, XinCurrentInterval, out lastXinCurrentInterval);

                // work inside interval - forward
                this.workAnInterval(d => +d, out x, ref points, XinCurrentInterval, out lastXinCurrentInterval);

                // finish up interval
                if (points.Count > 0)
                {
                    lastXinPreviousInterval = lastXinCurrentInterval;
                    retList.Add(points);
                }
            }

            return retList;
        }

        private void workAnInterval(Func<double, double> direction, out double x, ref ListPointLogical points, double XinCurrentInterval, out double xOld)
        {
            x = XinCurrentInterval + direction(epsilon);
            Maybe<double> yMaybe = this.expression.Evaluate(x);
            xOld = XinCurrentInterval;

            double y = yMaybe.HasValue ? yMaybe.Value : 0.0;
            double yOld = yMaybe.HasValue ? yMaybe.Value : this.expression.Evaluate(XinCurrentInterval).Value;

            double incr = PointListGenerator.initialIncrement;

            // scan the interval until it is interrupted
            bool interrupt = false;
            while (!interrupt)
            {
                interrupt = true;
                yMaybe = this.expression.Evaluate(x);

                if (yMaybe.HasValue)
                {
                    y = yMaybe.Value;
                    interrupt = !(initialX <= x && x <= finalX && -limits <= y && y <= limits);
                }

                if (!interrupt)
                {
                    double squareDist = Helper.MathHelper.SquareDistance(x, y, xOld, yOld);
                    if (squareDist < PointListGenerator.targetDistance || incr < PointListGenerator.epsilon)
                    {
                        if (squareDist < PointListGenerator.epsilon)
                        {
                            incr *= 2.0;
                        }

                        points.Add(new PointLogical(x, y));
                        xOld = x;
                        yOld = y;
                        x = x + direction(incr);
                    }
                    else
                    {
                        incr *= 0.5;
                        x = x - direction(incr);
                    }
                }
            }
        }
    }
}