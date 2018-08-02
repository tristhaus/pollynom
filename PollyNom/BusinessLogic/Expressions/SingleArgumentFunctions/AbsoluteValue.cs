using System;

namespace PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions
{
    public sealed class AbsoluteValue : SingleArgumentFunctionBase<AbsoluteValue>
    {
        internal static readonly string Symbol = "abs";

        /// <summary>
        /// Initializes a new instance of the <see cref="AbsoluteValue"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        public AbsoluteValue(IExpression containedExpression)
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
                return Math.Abs;
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
