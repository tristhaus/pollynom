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

namespace Backend.BusinessLogic
{
    /// <summary>
    /// Represents a point using implied logical business units.
    /// </summary>
    public class PointLogical : IComparable<PointLogical>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointLogical"/> class from the coordinates given.
        /// </summary>
        /// <param name="x">X-coordinate of the point in implied logical business units.</param>
        /// <param name="y">Y-coordinate of the point in implied logical business units.</param>
        public PointLogical(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets the x-coordinate in implied logical business units.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Gets the y-coordinate in implied logical business units.
        /// </summary>
        public double Y { get; }

        /// <inheritdoc />
        public int CompareTo(PointLogical other)
        {
            return this.X.CompareTo(other.X);
        }
    }
}
