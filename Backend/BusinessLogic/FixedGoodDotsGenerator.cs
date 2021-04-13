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

﻿using System.Collections.Generic;
using Persistence.Models;

namespace Backend.BusinessLogic
{
    /// <summary>
    /// Initializes good dots on a grid in a random manner.
    /// </summary>
    public class FixedGoodDotsGenerator
    {
        /// <summary>
        /// Generates a list of good dots
        /// </summary>
        /// <returns>A list of dots in terms of logical units.</returns>
        public List<IDot> Generate()
        {
            List<IDot> list = new List<IDot>();
            /*
            list.Add(new Dot(DotKind.Good, 0.0, 0.0));
            list.Add(new Dot(DotKind.Good, 4.0, 0.0));
            list.Add(new Dot(DotKind.Good, 1.0, 1.25));
            */

            list.Add(new Dot(DotKind.Good, -3.0, -3.0));
            /*
            list.Add(new Dot(DotKind.Good, -3.0, 3.0));
            list.Add(new Dot(DotKind.Good, 3.0, -3.0));
            list.Add(new Dot(DotKind.Good, 1.0, 0.0));
            list.Add(new Dot(DotKind.Good, 0.0, -1.0));
            */

            return list;
        }
    }
}
