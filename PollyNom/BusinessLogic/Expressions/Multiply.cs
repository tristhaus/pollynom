using System;
using System.Collections.Generic;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Multiply : Expression
    {
        private List<MultiplyExpression> list;

        public Multiply(Expression a, Expression b)
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

        public override bool IsMonadic
        {
            get
            {
                return false;
            }
        }

        public override int Level
        {
            get
            {
                return 2;
            }
        }

        public override Maybe<double> Evaluate(double input)
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

        public override Maybe<string> Print()
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

        public class MultiplyExpression : Expression
        {
            public enum Signs
            {
                Multiply,
                Divide
            }

            private Expression expression;

            public MultiplyExpression(Signs sign, Expression expression)
            {
                this.expression = expression;
                this.Sign = sign;
            }

            public Signs Sign { get; }

            public override bool IsMonadic => expression.IsMonadic;

            public override int Level => expression.Level;

            public override Maybe<double> Evaluate(double input)
            {
                return expression.Evaluate(input);
            }

            public override Maybe<string> Print()
            {
                return expression.Print();
            }
        }
    }
}
