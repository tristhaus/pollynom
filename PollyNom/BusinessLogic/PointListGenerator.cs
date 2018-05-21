﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace PollyNom.BusinessLogic
{
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
        private const double initialIncrement = 1e-2;

        private Evaluator evaluator;

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
        /// using an evaluator and parameters for the evaluation.
        /// </summary>
        /// <param name="evaluator">The evaluator to be used.</param>
        /// <param name="initialX">The x at which the list shall begin.</param>
        /// <param name="finalX">The x at which the list shall end.</param>
        /// <param name="limits">The absolute maximum y-value to be considered.</param>
        public PointListGenerator(Evaluator evaluator, double initialX, double finalX, double limits)
        {
            if (evaluator == null)
            {
                throw new ArgumentNullException(nameof(evaluator));
            }

            if (initialX >= finalX)
            {
                throw new ArgumentException($"{nameof(initialX)} must be smaller than {nameof(finalX)}");
            }

            this.evaluator = evaluator;
            this.initialX = initialX;
            this.finalX = finalX;
            this.limits = limits;
        }

        /// <summary>
        /// Provides a list of points ready for displaying,
        /// that respect the scaling factors provided.
        /// </summary>
        /// <param name="scaleX">Horizontal scaling factor.</param>
        /// <param name="scaleY">Vertical scaling factor.</param>
        /// <returns>A list of lists of points.</returns>
        public List<List<PointF>> ObtainScaledPoints(float scaleX = 1.0f, float scaleY = 1.0f)
        {
            var tupleLists = this.ObtainTuples();
            List<List<PointF>> pointLists = new List<List<PointF>>(tupleLists.Count);

            foreach (var tupleList in tupleLists)
            {
                List<PointF> pointList = new List<PointF>(tupleList.Count);
                foreach (var tuple in tupleList)
                {
                    try
                    {
                        PointF point = new PointF(scaleX*Convert.ToSingle(tuple.Key), scaleY*Convert.ToSingle(tuple.Value));
                        pointList.Add(point);
                    }
                    catch (OverflowException)
                    {
                    }
                }

                pointLists.Add(pointList);
            }

            return pointLists;
        }

        /// <summary>
        /// Provides access to a sorted list of points calculated from the expression
        /// using the member parameters in logical, business units.
        /// </summary>
        /// <returns>A list of sorted lists of points.</returns>
        private List<SortedList<double, double>> ObtainTuples()
        {
            List<SortedList<double, double>> retList = new List<SortedList<double, double>>();
            bool hadFirstPoint = false;
            double x = this.initialX;
            double y = 0;
            double xOld = x;
            double yOld = y;
            double incr = PointListGenerator.initialIncrement;

            SortedList<double, double> points = new SortedList<double, double>();

            do
            {
                var yMaybe = this.evaluator.Evaluate(Convert.ToDouble(x));

                bool interrupt = true;
                if (yMaybe.HasValue())
                {
                    y = yMaybe.Value();

                    if (hadFirstPoint)
                    {
                        interrupt = !(-limits <= x && x <= limits && -limits <= y && y <= limits);
                    }
                    else
                    {
                        interrupt = false;
                    }
                }

                if (!interrupt)
                {
                    double squareDist = this.squareDist(x, y, xOld, yOld);
                    if (!hadFirstPoint || squareDist < PointListGenerator.targetDistance || incr < PointListGenerator.epsilon)
                    {
                        if (squareDist < PointListGenerator.epsilon)
                        {
                            incr *= 2.0;
                        }

                        points.Add(x, y);
                        xOld = x;
                        yOld = y;
                        x += incr;
                    }
                    else
                    {
                        incr *= 0.5;
                        x -= incr;
                    }
                }
                else
                {
                    if (points.Count > 0)
                    {
                        retList.Add(points);
                        points = new SortedList<double, double>();
                    }
                    else
                    {
                        x += incr;
                    }
                }

                if (yMaybe.HasValue() && !hadFirstPoint)
                {
                    hadFirstPoint = true;
                }

            } while (x < this.finalX);

            if (points.Count > 0)
            {
                retList.Add(points);
                points = new SortedList<double, double>();
            }

            return retList;
        }

        /// <summary>
        /// Calculates the square of the Euclidean distance between
        /// two points provided as single coordinates.
        /// </summary>
        /// <param name="x1">X-coordinate of the first point.</param>
        /// <param name="y1">Y-coordinate of the first point.</param>
        /// <param name="x2">X-coordinate of the second point.</param>
        /// <param name="y2">Y-coordinate of the second point.</param>
        /// <returns>The squared distance.</returns>
        private double squareDist(double x1, double y1, double x2, double y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }
    }
}

