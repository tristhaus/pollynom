using System;
using System.Collections.Generic;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Multiply : IExpression
    {
        private List<MultiplyExpression> list;

        public Multiply(IExpression a, IExpression b)
        {
            this.list = new List<MultiplyExpression>(2);
            this.list.Add(new MultiplyExpression(MultiplyExpression.Signs.Multiply, a));
            this.list.Add(new MultiplyExpression(MultiplyExpression.Signs.Multiply, b));
        }

        public Multiply(params MultiplyExpression[] expressions)
        {
            this.list = new List<MultiplyExpression>();
            foreach (var expression in expressions)
            {
                this.list.Add(expression);
            }
        }

        protected Multiply(MultiplyExpression a, MultiplyExpression b)
        {
            this.list = new List<MultiplyExpression>(2);
            this.list.Add(a);
            this.list.Add(b);
        }

        public bool IsMonadic
        {
            get
            {
                return false;
            }
        }

        public int Level
        {
            get
            {
                return 2;
            }
        }

        public Maybe<double> Evaluate(double input)
        {
            double product = 1.0;

            foreach (var expression in this.list)
            {
                var value = expression.Evaluate(input);
                if (!value.HasValue() || (expression.Sign == MultiplyExpression.Signs.Divide && Math.Abs(value.Value()) < 10e-10))
                {
                    return new None<Double>();
                }
                product *= expression.Sign == MultiplyExpression.Signs.Multiply ? value.Value() : (1.0 / value.Value());
            }

            return new Some<double>(product);
        }

        public Maybe<string> Print()
        {
            string s = "1";

            foreach (var expression in this.list)
            {
                var value = expression.Print();
                if (!value.HasValue())
                {
                    return new None<string>();
                }

                var decoratedValue = value.Value();
                if (expression.Level == this.Level - 1)
                {
                    decoratedValue = "(" + decoratedValue + ")";
                }

                s += expression.Sign == MultiplyExpression.Signs.Multiply ? "*" + decoratedValue : "/" + decoratedValue;
            }

            if (s.StartsWith("1*"))
            {
                s = s.Remove(0, 2);
            }

            return new Some<string>(s);
        }

        public class MultiplyExpression : IExpression
        {
            public enum Signs
            {
                Multiply,
                Divide
            }

            private IExpression expression;

            public MultiplyExpression(Signs sign, IExpression expression)
            {
                this.expression = expression;
                this.Sign = sign;
            }

            public Signs Sign { get; }

            public bool IsMonadic => expression.IsMonadic;

            public int Level => expression.Level;

            public Maybe<double> Evaluate(double input)
            {
                return expression.Evaluate(input);
            }

            public Maybe<string> Print()
            {
                return expression.Print();
            }
        }
    }
}
