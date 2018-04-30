using PollyNom.BusinessLogic;

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
        internal static Expression Expression01()
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
        internal static Expression Expression02()
        {
            return new Divide(new Multiply(new Constant(2.0), new Power(new BaseX(), new Constant(3.0))), new Subtract(new BaseX(), new Power(new Constant(2.0), new BaseX())));
        }
    }
}
