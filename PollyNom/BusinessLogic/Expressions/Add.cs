using System;
using System.Collections.Generic;
using System.Linq;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Add : IExpression, IEquatable<Add>
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

        public Add(List<AddExpression> list)
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
                return 1;
            }
        }

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(Add))
            {
                return false;
            }
            Add otherAdd = (Add)other;

            return this.equalityImplementation(otherAdd);
        }

        public bool Equals(Add other)
        {
            return this.equalityImplementation(other);
        }

        public static bool operator ==(Add x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Add x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(Add x, Add y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Add x, Add y)
        {
            return !(x.Equals(y));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int finalHash = 17;
                List<int> hashes = new List<int>(list.Count);
                foreach (var addExpression in list)
                {
                    hashes.Add(addExpression.GetHashCode());
                }
                hashes.Sort();
                foreach (var hash in hashes)
                {
                    finalHash = finalHash * 23 + hash;
                }
                return finalHash;
            }
        }

        public Maybe<double> Evaluate(double input)
        {
            double sum = 0.0;

            foreach(var expression in this.list)
            {
                var value = expression.Evaluate(input);
                if (!value.HasValue)
                {
                    return new None<Double>();
                }
                sum += expression.Sign == AddExpression.Signs.Plus ? (+value.Value) : (-value.Value);
            }

            return new Some<double>(sum);
        }

        public Maybe<string> Print()
        {
            string s = string.Empty;

            foreach (var expression in this.list)
            {
                var value = expression.Print();
                if (!value.HasValue)
                {
                    return new None<string>();
                }
                s += expression.Sign == AddExpression.Signs.Plus ? "+" + value.Value: "-" + value.Value;
            }

            if(s[0] == '+')
            {
                s = s.Remove(0, 1);
            }

            return new Some<string>(s);
        }

        private bool equalityImplementation(Add other)
        {
            if (this.list.Count != other.list.Count)
            {
                return false;
            }

            var cnt = new Dictionary<AddExpression, int>();
            foreach (AddExpression s in this.list)
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

            foreach (AddExpression s in other.list)
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

        public class AddExpression : IExpression, IEquatable<AddExpression>
        {
            public enum Signs
            {
                Plus = 1,
                Minus = 2
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

            public override bool Equals(object other)
            {
                if (other.GetType() != typeof(AddExpression))
                {
                    return false;
                }
                AddExpression otherAddExpression = (AddExpression)other;
                return this.Sign == otherAddExpression.Sign && this.expression.Equals(otherAddExpression.expression);
            }

            public bool Equals(AddExpression other)
            {
                return this.Sign == other.Sign && this.expression.Equals(other.expression);
            }

            public static bool operator ==(AddExpression x, IExpression y)
            {
                return x.Equals(y);
            }

            public static bool operator !=(AddExpression x, IExpression y)
            {
                return !(x.Equals(y));
            }

            public static bool operator ==(AddExpression x, AddExpression y)
            {
                return x.Equals(y);
            }

            public static bool operator !=(AddExpression x, AddExpression y)
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
