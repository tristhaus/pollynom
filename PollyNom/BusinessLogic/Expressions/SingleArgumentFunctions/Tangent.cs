using System;

namespace PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions
{
    public sealed class Tangent : SingleArgumentFunctionBase<Tangent>
    {
        internal static readonly string Symbol = "tan";

        /// <summary>
        /// Initializes a new instance of the <see cref="Tangent"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        public Tangent(IExpression containedExpression)
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
                return Math.Tan;
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
