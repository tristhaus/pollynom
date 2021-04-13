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
    /// Defines a dot, which can be hit by an expression and
    /// gives information relevant to its drawing.
    /// </summary>
    public interface IDrawDot
    {
        /// <summary>
        /// Gets the position of the center of the dot in logical business units.
        /// </summary>
        Tuple<double, double> Position { get; }

        /// <summary>
        /// Gets the radius of the dot in logical business units.
        /// </summary>
        double Radius { get; }

        /// <summary>
        /// Gets a value indicating whether the dot was hit.
        /// </summary>
        /// <returns><c>true</c> if hit, <c>false</c> otherwise.</returns>
        bool IsHit { get; }

        /// <summary>
        ///  Gets the kind of the dot, as defined in <see cref="DotKind"/>.
        /// </summary>
        DotKind Kind { get; }
    }
}