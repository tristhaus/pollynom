using System;
using System.Collections.Generic;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Add : Expression
    {
        private List<AddExpression> list;

        public Add(Expression a, Expression b)
        {
            this.list = new List<AddExpression>(2);
            this.list.Add(new AddExpression(AddExpression.Signs.Plus, a));
            this.list.Add(new AddExpression(AddExpression.Signs.Plus, b));
        }

        public Add(params AddExpression[] expressions)
        {
            this.list = new List<AddExpression>();
            foreach(var expression in expressions)
            {
                this.list.Add(expression);
            }
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
                return 1;
            }
        }

        public override Maybe<double> Evaluate(double input)
        {
            double sum = 0.0;

            foreach(var expression in this.list)
            {
                var value = expression.Evaluate(input);
                if (!value.HasValue())
                {
                    return new None<Double>();
                }
                sum += expression.Sign == AddExpression.Signs.Plus ? (+value.Value()) : (-value.Value()); 
            }

            return new Some<double>(sum);
        }

        public override Maybe<string> Print()
        {
            string s = string.Empty;

            foreach (var expression in this.list)
            {
                var value = expression.Print();
                if (!value.HasValue())
                {
                    return new None<string>();
                }
                s += expression.Sign == AddExpression.Signs.Plus ? "+" + value.Value() : "-" + value.Value();
            }

            if(s[0] == '+')
            {
                s = s.Remove(0, 1);
            }

            return new Some<string>(s);
        }

        public class AddExpression : Expression
        {
            public enum Signs
            {
                Plus,
                Minus
            }

            private Expression expression;

            public AddExpression(Signs sign, Expression expression)
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
