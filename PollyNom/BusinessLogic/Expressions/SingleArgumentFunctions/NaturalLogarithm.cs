using System;

namespace PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions
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

        protected override bool ArgumentIsValid(double argument)
        {
            return argument > 0.0;
        }

        protected override Func<double, double> FunctionFunc
        {
            get
            {
                return Math.Log;
            }
        }

        protected override string FunctionSymbol
        {
            get
            {
                return Symbol;
            }
        }
    }
}
