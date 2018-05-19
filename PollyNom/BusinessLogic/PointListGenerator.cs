using System;
using System.Collections.Generic;
using System.Drawing;

namespace PollyNom.BusinessLogic
{
    public class PointListGenerator
    {
        private const double epsilon = 1e-4;
        private const double targetDistance = 5e-3;
        private const double initialIncrement = 1e-2;

        private Evaluator evaluator;
        private double initialX;
        private double finalX;
        private double limits;

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

        private double squareDist(double x1, double y1, double x2, double y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }
    }
}

