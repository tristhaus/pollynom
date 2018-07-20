using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public sealed class Exponential : SingleArgumentFunctionBase<Exponential>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Exponential"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        public Exponential(IExpression containedExpression) : base(containedExpression)
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
                return "exp";
            }
        }
    }
}
