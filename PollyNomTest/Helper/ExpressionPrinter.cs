using PollyNom.BusinessLogic;

namespace PollyNomTest.Helper
{
    internal class ExpressionPrinter
    {
        public static string PrintExpression(IExpression expression)
        {
            return expression.Print().HasValue() ? expression.Print().Value() : "invalid";
        }
    }
}
