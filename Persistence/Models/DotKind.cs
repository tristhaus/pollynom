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

﻿namespace Persistence.Models
{
    /// <summary>
    /// Enumerates the kinds of dots available.
    /// </summary>
    public enum DotKind
    {
        /// <summary>
        /// A good dot that the player wants to hit.
        /// </summary>
        Good,

        /// <summary>
        /// A bad dot that the player does not want to hit.
        /// </summary>
        Bad,
    }
}
