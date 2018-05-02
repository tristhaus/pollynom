using System;
using System.Collections.Generic;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Add : IExpression
    {
        private List<AddExpression> list;

        public Add(IExpression a, IExpression b)
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

        protected Add(AddExpression a, AddExpression b)
        {
            this.list = new List<AddExpression>(2);
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
                return 1;
            }
        }

        public Maybe<double> Evaluate(double input)
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

        public Maybe<string> Print()
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

        public class AddExpression : IExpression
        {
            public enum Signs
            {
                Plus,
                Minus
            }

            private IExpression expression;

            public AddExpression(Signs sign, IExpression expression)
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
