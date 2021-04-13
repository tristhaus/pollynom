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

namespace Backend.BusinessLogic
{
    /// <summary>
    /// Represents a sorted list of <see cref="PointLogical"/>. Is kept sorted on insertion.
    /// </summary>
    public class ListPointLogical
    {
        private readonly List<PointLogical> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListPointLogical"/> class.
        /// </summary>
        public ListPointLogical()
        {
            this.list = new List<PointLogical>();
        }

        /// <summary>
        /// Gets the list of points contained in this instance.
        /// </summary>
        public IReadOnlyList<PointLogical> Points
        {
            get
            {
                return this.list.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the count of points contained in this instance.
        /// </summary>
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        /// <summary>
        /// Adds the given point to the list, keeping the list sorted as needed.
        /// </summary>
        /// <param name="point">The point to be added.</param>
        public void Add(PointLogical point)
        {
            if (this.list.Count == 0 || this.list[this.list.Count - 1].X < point.X)
            {
                this.list.Add(point);
                return;
            }
            else
            {
                this.list.Add(point);
                this.list.Sort();
            }
        }
    }
}
