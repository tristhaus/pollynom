using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class NaturalLogarithm : SingleArgumentFunctionBase<NaturalLogarithm>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="NaturalLogarithm"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        public NaturalLogarithm(IExpression containedExpression) : base(containedExpression)
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
                return "ln";
            }
        }
    }
}
