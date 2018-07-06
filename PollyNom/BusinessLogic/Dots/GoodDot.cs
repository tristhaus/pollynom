using System;
using System.Collections.Generic;

namespace PollyNom.BusinessLogic.Dots
{
    public class GoodDot : IDot
    {
        private static Random rng = new Random();

        private const double radius = 0.25;
        private readonly double x;
        private readonly double y;

        public GoodDot(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Tuple<double, double> Position
        {
            get
            {
                return new Tuple<double, double>(this.x, this.y);
            }
        }

        public double Radius
        {
            get
            {
                return GoodDot.radius;
            }
        }

        public bool IsHit(IExpression expression, List<SortedList<double, double>> tupleLists)
        {
            if(expression.GetType() == typeof(BusinessLogic.Expressions.InvalidExpression))
            {
                return false;
            }

            return this.HitImplementationListBased(tupleLists) || this.HitImplementationGreedy(expression);
        }

        private bool HitImplementationListBased(List<SortedList<double, double>> tupleLists)
        {
            if (tupleLists == null) return false;

            bool retval = false;
            foreach (var tupleList in tupleLists)
            {
                foreach (var tuple in tupleList)
                {
                    if(tuple.Key >= this.x - this.Radius && tuple.Key <= this.x + this.Radius)
                    {
                        if (this.IsInsideCircle(tuple.Key, tuple.Value))
                        {
                            retval = true;
                            break;
                        }
                    }
                }
                if (retval) break;
            }

            return retval;
        }

        private bool HitImplementationGreedy(IExpression expression)
        {
            double mid = this.x;
            double increment = this.Radius;
            const double epsilon = 1e-6;
            const double dampingFactor = 0.8;
            bool retval = false;

            int iterations = 0;

            while(!retval && increment > epsilon)
            {
                var midEvaluationResult = expression.Evaluate(mid);
                if(midEvaluationResult.HasValue())
                {
                    if(this.IsInsideCircle(mid, midEvaluationResult.Value()))
                    {
                        retval = true;
                        break;
                    }
                }

                var rightEvaluationResult = expression.Evaluate(mid + increment);
                if(rightEvaluationResult.HasValue())
                {
                    if (this.IsInsideCircle(mid + increment, rightEvaluationResult.Value()))
                    {
                        retval = true;
                        break;
                    }
                }

                var leftEvaluationResult = expression.Evaluate(mid - increment);
                if (leftEvaluationResult.HasValue())
                {
                    if (this.IsInsideCircle(mid - increment, leftEvaluationResult.Value()))
                    {
                        retval = true;
                        break;
                    }
                }

                // well, that was easy. Now the hard part.
                if(midEvaluationResult.HasValue() && rightEvaluationResult.HasValue() && leftEvaluationResult.HasValue())
                {
                    var leftGradient = Helper.MathHelper.SquareDistance(this.x, this.y, mid - increment, leftEvaluationResult.Value());
                    var rightGradient = Helper.MathHelper.SquareDistance(this.x, this.y, mid + increment, rightEvaluationResult.Value());

                    if (leftGradient < rightGradient)
                    {
                        increment *= leftGradient < 10 ? leftGradient / 20 : 0.75;
                        mid -= increment;
                    }
                    else
                    {
                        increment *= rightGradient < 10 ? rightGradient / 20 : 0.75;
                        mid += increment;
                    }
                    continue;
                }

                // from now on, we are in desperate trouble, and eventually skip out
                iterations++;

                // in the following cases, damp the movement
                if (midEvaluationResult.HasValue() && rightEvaluationResult.HasValue())
                {
                    mid += increment * dampingFactor;
                    increment *= 0.5;
                    continue;
                }
                else if (midEvaluationResult.HasValue() && leftEvaluationResult.HasValue())
                {
                    mid -= increment * dampingFactor;
                    increment *= 0.5;
                    continue;
                }
                else if (rightEvaluationResult.HasValue())
                {
                    mid += increment * dampingFactor;
                    continue;
                }
                else if (leftEvaluationResult.HasValue())
                {
                    mid -= increment * dampingFactor;
                    continue;
                }

                // if everything else fails, pick a new random Mid somewhere in the dot
                double rand = GoodDot.rng.NextDouble();
                mid = this.x + this.Radius * 0.75 * (rand - 0.5);
            }

            return retval;
        }

        private bool IsInsideCircle(double otherX, double otherY)
        {
            return (this.Radius * this.Radius) >= Helper.MathHelper.SquareDistance(this.x, this.y, otherX, otherY); 
        }
    }
}
