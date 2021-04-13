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
using System.Linq;

namespace Backend.BusinessLogic
{
    /// <summary>
    /// A class implementing the calculation of the score based on lists of hit counts.
    /// </summary>
    public static class ScoreCalculator
    {
        /// <summary>
        /// Calculates the score according to the series of numbers of hits.
        /// </summary>
        /// <param name="numbersOfGoodHits">List of good hit series.</param>
        /// <param name="numbersOfBadHits">List of bad hit series.</param>
        /// <returns>The calculated score.</returns>
        public static int CalculateScore(List<int> numbersOfGoodHits, List<int> numbersOfBadHits)
        {
            if (numbersOfBadHits.Any(x => x > 0))
            {
                return -1;
            }

            return numbersOfGoodHits.Where(x => (x > 0)).Select(x => ((int)Math.Pow(2, x) - 1)).Sum();
        }
    }
}
