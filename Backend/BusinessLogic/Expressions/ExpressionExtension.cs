namespace Backend.BusinessLogic.Expressions
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
