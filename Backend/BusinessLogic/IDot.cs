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
    /// <summary>
    /// Defines a dot, which can be hit by an expression and
    /// gives information relevant to its drawing.
    /// </summary>
    public interface IDot
    {
        /// <summary>
        /// Gets the position of the center of the dot in logical business units.
        /// </summary>
        Tuple<double, double> Position { get; }

        /// <summary>
        /// Gets the kind of the dot.
        /// </summary>
        DotKind Kind { get; }

        /// <summary>
        /// Gets the radius of the dot in logical business units.
        /// </summary>
        double Radius { get; }

        /// <summary>
        /// Indicates whether the provided expression (also represented as a series of pre-evaluated points) hits the dot.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <param name="logicalPointLists">Lists of pre-evaluated points</param>
        /// <returns><c>true</c> if hit, <c>false</c> otherwise.</returns>
        bool IsHit(IExpression expression, List<ListPointLogical> logicalPointLists);

        /// <summary>
        /// Provides the model representing this dot.
        /// </summary>
        /// <returns>The model.</returns>
        DotModel GetModel();
    }
}
