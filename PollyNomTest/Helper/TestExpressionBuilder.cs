using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Expressions;
using System.Collections.Generic;

namespace PollyNomTest.Helper
{
    internal class TestExpressionBuilder
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
            return new Divide(new Multiply(new Constant(2.0), new Power(new BaseX(), new Constant(3.0))), new Subtract(new BaseX(), new Power(new Constant(2.0), new BaseX())));
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
            return new Power(new Add(new BaseX(), new Constant(1.0)), new Divide(new BaseX(), new Constant(3.0)));
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
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(3.0))
                );
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
            Add.AddExpression[] expressions = {
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
