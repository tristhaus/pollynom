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

﻿namespace Backend.Controller
{
    /// <summary>
    /// DTO concerning the coordinate system
    /// </summary>
    public class CoordinateSystemInfo
    {
        /// <summary>
        /// Gets or sets the left end of the x-axis in business logical units.
        /// </summary>
        public float StartX { get; set; }

        /// <summary>
        /// Gets or sets the right end of the x-axis in business logical units.
        /// </summary>
        public float EndX { get; set; }

        /// <summary>
        /// Gets or sets the bottom end of the x-axis in business logical units.
        /// </summary>
        public float StartY { get; set; }

        /// <summary>
        /// Gets or sets the top end of the x-axis in business logical units.
        /// </summary>
        public float EndY { get; set; }

        /// <summary>
        /// Gets or sets the distance between ticks.
        /// </summary>
        public float TickInterval { get; set; }
    }
}
