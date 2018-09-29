﻿using System;

namespace Backend.BusinessLogic.Expressions.SingleArgumentFunctions
{
    public sealed class Cosine : SingleArgumentFunctionBase<Cosine>
    {
        internal static readonly string Symbol = "cos";

        /// <summary>
        /// Initializes a new instance of the <see cref="Cosine"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        public Cosine(IExpression containedExpression)
            : base(containedExpression)
        {
        }

        protected sealed override string FunctionSymbol
        {
            get
            {
                return Symbol;
            }
        }

        protected sealed override Func<double, double> FunctionFunc
        {
            get
            {
                return Math.Cos;
            }
        }

        protected override bool ArgumentIsValid(double argument)
        {
            return true;
        }
    }
}