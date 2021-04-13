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

namespace Backend.BusinessLogic.Expressions.SingleArgumentFunctions
{
    /// <summary>
    /// Implements the natural logarithm, i.e. base <c>e ~ 2.71828</c>.
    /// </summary>
    public sealed class NaturalLogarithm : SingleArgumentFunctionBase<NaturalLogarithm>
    {
        public static readonly string Symbol = "ln";

        /// <summary>
        /// Initializes a new instance of the <see cref="NaturalLogarithm"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        public NaturalLogarithm(IExpression containedExpression)
            : base(containedExpression)
        {
        }

        protected override string FunctionSymbol
        {
            get
            {
                return Symbol;
            }
        }

        protected override Func<double, double> FunctionFunc
        {
            get
            {
                return Math.Log;
            }
        }

        protected override bool ArgumentIsValid(double argument)
        {
            return argument > 0.0;
        }
    }
}
