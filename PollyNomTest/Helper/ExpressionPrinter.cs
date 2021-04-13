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

﻿using Backend.BusinessLogic;

namespace PollyNomTest.Helper
{
    /// <summary>
    /// Provides a filtered printing of expressions.
    /// </summary>
    internal static class ExpressionPrinter
    {
        /// <summary>
        /// Print the expression, filter the result.
        /// </summary>
        /// <param name="expression">The expression to be printed and filtered</param>
        /// <returns>The expression string or <c>"invalid"</c></returns>
        public static string PrintExpression(IExpression expression)
        {
            return expression.Print().HasValue ? expression.Print().Value : "invalid";
        }
    }
}
