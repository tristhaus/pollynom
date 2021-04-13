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

﻿namespace Backend.BusinessLogic.Expressions
{
    /// <summary>
    /// This class collects extension methods to the <see cref="IExpression"/> implementations.
    /// </summary>
    public static class ExpressionExtension
    {
        private static readonly InvalidExpression InvalidExpression = new InvalidExpression();

        /// <summary>
        /// Tests whether a given expression is null or equal to InvalidExpression.
        /// </summary>
        /// <param name="expression">The expression to be tested.</param>
        /// <returns><c>true</c> if null or equal to InvalidExpression.</returns>
        public static bool IsNullOrInvalidExpression(this IExpression expression)
        {
            return expression == null || expression.Equals(InvalidExpression);
        }
    }
}
