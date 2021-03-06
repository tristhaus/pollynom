/*
 * This file is part of PollyNom.
 * 
 * PollyNom is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * PollyNom is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with PollyNom.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

﻿using System;
using System.Collections.Generic;
using Persistence.Models;

namespace Backend.BusinessLogic
{
    public class Dot : IDot
    {
        /// <summary>
        /// Radius of the dot in implied logical business units.
        /// </summary>
        private const double RadiusValue = 0.25;

        /// <summary>
        /// Random number generator
        /// </summary>
        private static readonly Random rng = new Random();

        /// <summary>
        /// X-coordinate of the dot in implied logical business units.
        /// </summary>
        private readonly double x;

        /// <summary>
        /// Y-coordinate of the dot in implied logical business units.
        /// </summary>
        private readonly double y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dot"/> class from the model given.
        /// </summary>
        /// <param name="model">The model representing this dot.</param>
        public Dot(DotModel model)
            : this(model.Kind, model.X, model.Y)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dot"/> class from the kind and coordinates given.
        /// </summary>
        /// <param name="kind">The kind of the dot.</param>
        /// <param name="x">X-coordinate of the dot in implied logical business units.</param>
        /// <param name="y">Y-coordinate of the dot in implied logical business units.</param>
        public Dot(DotKind kind, double x, double y)
        {
            this.Kind = kind;
            this.x = x;
            this.y = y;
        }

        /// <inheritdoc />
        public Tuple<double, double> Position
        {
            get
            {
                return new Tuple<double, double>(this.x, this.y);
            }
        }

        /// <inheritdoc />
        public DotKind Kind { get; }

        /// <inheritdoc />
        public double Radius => Dot.RadiusValue;

        /// <inheritdoc />
        public bool IsHit(IExpression expression, List<ListPointLogical> tupleLists)
        {
            if (expression.GetType() == typeof(BusinessLogic.Expressions.InvalidExpression))
            {
                return false;
            }

            return this.HitImplementationListBased(tupleLists) || this.HitImplementationGreedy(expression);
        }

        public DotModel GetModel()
        {
            return new DotModel() { Kind = this.Kind, X = this.x, Y = this.y };
        }

        private bool HitImplementationListBased(List<ListPointLogical> tupleLists)
        {
            if (tupleLists == null)
            {
                return false;
            }

            bool retval = false;
            foreach (var tupleList in tupleLists)
            {
                foreach (var tuple in tupleList.Points)
                {
                    if (tuple.X >= this.x - this.Radius)
                    {
                        if (this.IsInsideCircle(tuple.X, tuple.Y))
                        {
                            retval = true;
                            break;
                        }

                        if (tuple.X > this.x + this.Radius)
                        {
                            break;
                        }
                    }
                }

                if (retval)
                {
                    break;
                }
            }

            return retval;
        }

        private bool HitImplementationGreedy(IExpression expression)
        {
            const double epsilon = 1e-6;
            const double dampingFactor = 0.8;
            const uint maxIterations = 10000;

            double mid = this.x;
            double increment = this.Radius;
            bool retval = false;

            uint iterations = 0;

            while (!retval && increment > epsilon && iterations < maxIterations)
            {
                var midEvaluationResult = expression.Evaluate(mid);
                if (midEvaluationResult.HasValue)
                {
                    if (this.IsInsideCircle(mid, midEvaluationResult.Value))
                    {
                        retval = true;
                        break;
                    }
                }

                var rightEvaluationResult = expression.Evaluate(mid + increment);
                if (rightEvaluationResult.HasValue)
                {
                    if (this.IsInsideCircle(mid + increment, rightEvaluationResult.Value))
                    {
                        retval = true;
                        break;
                    }
                }

                var leftEvaluationResult = expression.Evaluate(mid - increment);
                if (leftEvaluationResult.HasValue)
                {
                    if (this.IsInsideCircle(mid - increment, leftEvaluationResult.Value))
                    {
                        retval = true;
                        break;
                    }
                }

                // well, that was easy. Now the hard part.
                if (midEvaluationResult.HasValue && rightEvaluationResult.HasValue && leftEvaluationResult.HasValue)
                {
                    var leftGradient = Helper.MathHelper.SquareDistance(this.x, this.y, mid - increment, leftEvaluationResult.Value);
                    var rightGradient = Helper.MathHelper.SquareDistance(this.x, this.y, mid + increment, rightEvaluationResult.Value);

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
                if (midEvaluationResult.HasValue && rightEvaluationResult.HasValue)
                {
                    mid += increment * dampingFactor;
                    increment *= 0.5;
                    continue;
                }
                else if (midEvaluationResult.HasValue && leftEvaluationResult.HasValue)
                {
                    mid -= increment * dampingFactor;
                    increment *= 0.5;
                    continue;
                }
                else if (rightEvaluationResult.HasValue)
                {
                    mid += increment * dampingFactor;
                    continue;
                }
                else if (leftEvaluationResult.HasValue)
                {
                    mid -= increment * dampingFactor;
                    continue;
                }

                // if everything else fails, pick a new random Mid somewhere in the dot
                double rand = Dot.rng.NextDouble();
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
