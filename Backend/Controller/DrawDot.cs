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
using Persistence.Models;

namespace Backend.Controller
{
    /// <summary>
    /// A drawable dot.
    /// </summary>
    public class DrawDot : IDrawDot
    {
        private readonly double xCoordinate;
        private readonly double yCoordinate;
        private readonly double radius;
        private readonly DotKind kind;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawDot"/> class.
        /// </summary>
        /// <param name="xCoordinate">X-coordinate of the dot.</param>
        /// <param name="yCoordinate">Y-coordinate of the dot.</param>
        /// <param name="radius">Radius of the dot.</param>
        /// <param name="kind">The kind of the dot.</param>
        public DrawDot(double xCoordinate, double yCoordinate, double radius, DotKind kind)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.radius = radius;
            this.kind = kind;

            this.IsHit = false;
        }

        /// <summary>
        /// Gets the position of the dot as an x,y tuple.
        /// </summary>
        public Tuple<double, double> Position
        {
            get
            {
                return new Tuple<double, double>(this.xCoordinate, this.yCoordinate);
            }
        }

        /// <summary>
        /// Gets the radius of the dot.
        /// </summary>
        public double Radius
        {
            get
            {
                return this.radius;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dot has been hit.
        /// </summary>
        public bool IsHit { get; set; }

        /// <summary>
        /// Gets the kind of the dot.
        /// </summary>
        public DotKind Kind
        {
            get
            {
                return this.kind;
            }
        }
    }
}
