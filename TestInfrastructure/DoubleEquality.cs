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

namespace TestInfrastructure
{
    /// <summary>
    /// Allows to compare two doubles for approximate equality.
    /// </summary>
    public static class DoubleEquality
    {
        /// <summary>
        /// Threshold below which the absolute difference must be for equality.
        /// </summary>
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Compare the provided doubles for approximate equality.
        /// </summary>
        /// <param name="a">First double to be compared.</param>
        /// <param name="b">Second double to be compared.</param>
        /// <returns><c>true</c> if the doubles are close enough to each other.</returns>
        public static bool IsApproximatelyEqual(double a, double b)
        {
            return (Math.Abs(a - b)) < DoubleEquality.Epsilon;
        }
    }
}
