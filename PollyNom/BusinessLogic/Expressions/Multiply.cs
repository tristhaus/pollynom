using System;
using System.Collections.Generic;
using System.Linq;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Multiply : IExpression, IEquatable<Multiply>
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

        public Multiply(List<MultiplyExpression> list)
        {
            this.list = list;
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

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(Multiply))
            {
                return false;
            }
            Multiply otherMultiply = (Multiply)other;

            return this.equalityImplementation(otherMultiply);
        }

        public bool Equals(Multiply other)
        {
            return this.equalityImplementation(other);
        }

        public static bool operator ==(Multiply x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Multiply x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(Multiply x, Multiply y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Multiply x, Multiply y)
        {
            return !(x.Equals(y));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var multiplyExpression in list)
                {
                    hash = hash * 23 + multiplyExpression.GetHashCode();
                }
                return hash;
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

        private bool equalityImplementation(Multiply other)
        {
            if (this.list.Count != other.list.Count)
            {
                return false;
            }

            var cnt = new Dictionary<MultiplyExpression, int>();
            foreach (MultiplyExpression s in this.list)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }

            foreach (MultiplyExpression s in other.list)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }

            return cnt.Values.All(c => c == 0);
        }

        public class MultiplyExpression : IExpression, IEquatable<MultiplyExpression>
        {
            public enum Signs
            {
                Multiply = 3,
                Divide = 4
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

            public override bool Equals(object other)
            {
                if (other.GetType() != typeof(MultiplyExpression))
                {
                    return false;
                }
                MultiplyExpression otherMultiplyExpression = (MultiplyExpression)other;
                return this.Sign == otherMultiplyExpression.Sign && this.expression.Equals(otherMultiplyExpression.expression);
            }

            public bool Equals(MultiplyExpression other)
            {
                return this.Sign == other.Sign && this.expression.Equals(other.expression);
            }

            public static bool operator ==(MultiplyExpression x, IExpression y)
            {
                return x.Equals(y);
            }

            public static bool operator !=(MultiplyExpression x, IExpression y)
            {
                return !(x.Equals(y));
            }

            public static bool operator ==(MultiplyExpression x, MultiplyExpression y)
            {
                return x.Equals(y);
            }

            public static bool operator !=(MultiplyExpression x, MultiplyExpression y)
            {
                return !(x.Equals(y));
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + this.Sign.GetHashCode();
                    hash = hash * 23 + this.expression.GetHashCode();
                    return hash;
                }
            }

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
