using System;

namespace PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions
{
    public sealed class Sine : SingleArgumentFunctionBase<Sine>
    {
        internal static readonly string Symbol = "sin";

        /// <summary>
        /// Initializes a new instance of the <see cref="Sine"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        public Sine(IExpression containedExpression)
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
                return Math.Sin;
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
