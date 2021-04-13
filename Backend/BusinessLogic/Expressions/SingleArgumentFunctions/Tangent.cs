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
    public sealed class Tangent : SingleArgumentFunctionBase<Tangent>
    {
        internal static readonly string Symbol = "tan";

        /// <summary>
        /// Initializes a new instance of the <see cref="Tangent"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        public Tangent(IExpression containedExpression)
            : base(containedExpression)
        {
        }

        protected sealed override string FunctionSymbol
        {
            get
            {
                return Symbol;
            }
        }

        protected sealed override Func<double, double> FunctionFunc
        {
            get
            {
                return Math.Tan;
            }
        }

        protected override bool ArgumentIsValid(double argument)
        {
            return true;
        }
    }
}
