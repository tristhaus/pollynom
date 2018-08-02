using System;

namespace PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions
{
    /// <summary>
    /// Implements the exponential function,
    /// i.e. base <c>e ~ 2.71828</c> raised to the argument.
    /// </summary>
    public sealed class Exponential : SingleArgumentFunctionBase<Exponential>
    {
        internal static readonly string Symbol = "exp";

        /// <summary>
        /// Initializes a new instance of the <see cref="Exponential"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        public Exponential(IExpression containedExpression)
            : base(containedExpression)
        {
        }

        protected override bool ArgumentIsValid(double argument)
        {
            return true;
        }

        protected sealed override Func<double, double> FunctionFunc
        {
            get
            {
                return Math.Exp;
            }
        }

        protected sealed override string FunctionSymbol
        {
            get
            {
                return Symbol;
            }
        }
    }
}
