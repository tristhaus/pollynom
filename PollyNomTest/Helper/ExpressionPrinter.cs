using PollyNom.BusinessLogic;

namespace PollyNomTest.Helper
{
    /// <summary>
    /// Provides a filtered printing of expressions.
    /// </summary>
    internal class ExpressionPrinter
    {
        /// <summary>
        /// Print the expression, filter the result.
        /// </summary>
        /// <param name="expression">The expression to be printed and filtered</param>
        /// <returns>The expression string or <c>"invalid"</c></returns>
        public static string PrintExpression(IExpression expression)
        {
            return expression.Print().HasValue() ? expression.Print().Value() : "invalid";
        }
    }
}
