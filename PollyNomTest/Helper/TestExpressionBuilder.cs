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
using Backend.BusinessLogic;
using Backend.BusinessLogic.Expressions;

namespace PollyNomTest.Helper
{
    /// <summary>
    /// Provides a collection of test expressions.
    /// </summary>
    internal static class TestExpressionBuilder
    {
        /// <summary>
        /// Builds
        /// <c>
        /// 2.0*x
        /// </c>
        /// </summary>
        /// <returns>An Expression object.</returns>
        internal static IExpression Expression01()
        {
            return new Multiply(new Constant(2.0), new BaseX());
        }

        /// <summary>
        /// Builds
        /// <c>
        /// 2.0*x^3.0/(x-2^x)
        /// </c>
        /// </summary>
        /// <returns>An Expression object.</returns>
        internal static IExpression Expression02()
        {
            return new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Power(new BaseX(), new Constant(3.0))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Power(new Constant(2.0), new BaseX())))));
        }

        /// <summary>
        /// Builds
        /// <c>
        /// 2.0*(x+1.0)
        /// </c>
        /// </summary>
        /// <returns>An Expression object.</returns>
        internal static IExpression Expression03()
        {
            return new Multiply(new Constant(2.0), new Add(new BaseX(), new Constant(1.0)));
        }

        /// <summary>
        /// Builds
        /// <c>
        /// (x+1.0)^2.0
        /// </c>
        /// </summary>
        /// <returns>An Expression object.</returns>
        internal static IExpression Expression04()
        {
            return new Power(new Add(new BaseX(), new Constant(1.0)), new Constant(2.0));
        }

        /// <summary>
        /// Builds
        /// <c>
        /// (x+1.0)^(x/3.0)
        /// </c>
        /// </summary>
        /// <returns>An Expression object.</returns>
        internal static IExpression Expression05()
        {
            return new Power(new Add(new BaseX(), new Constant(1.0)), new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(3.0))));
        }

        /// <summary>
        /// Builds
        /// <c>
        /// x-1+2-3
        /// </c>
        /// </summary>
        /// <returns>An Expression object.</returns>
        internal static IExpression Expression06()
        {
            return new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(2.0)),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(3.0)));
        }

        /// <summary>
        /// Builds
        /// <c>
        /// x+1-2+3
        /// </c>
        /// </summary>
        /// <returns>An Expression object.</returns>
        internal static IExpression Expression07()
        {
            Add.AddExpression[] expressions =
            {
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0)),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(2.0)),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.0))
            };

            return new Add(expressions);
        }

        /// <summary>
        /// Builds
        /// <c>
        /// x+1-4+7
        /// </c>
        /// </summary>
        /// <returns>An Expression object.</returns>
        internal static IExpression Expression08()
        {
            List<Add.AddExpression> list = new List<Add.AddExpression>();

            list.Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()));
            list.Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0)));
            list.Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(4.0)));
            list.Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(7.0)));

            return new Add(list.ToArray());
        }
    }
}
