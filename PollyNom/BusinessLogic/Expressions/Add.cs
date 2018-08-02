using System;
using System.Collections.Generic;
using System.Linq;

namespace PollyNom.BusinessLogic.Expressions
{
    /// <summary>
    /// Implements a sum over an arbritrary, fixed list of summands.
    /// </summary>
    public sealed class Add : IExpression, IEquatable<Add>
    {
        private List<AddExpression> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="Add"/> class.
        /// </summary>
        /// <param name="a">First summand of the resulting expression.</param>
        /// <param name="b">Second and last summand of the resulting expression.</param>
        public Add(IExpression a, IExpression b)
        {
            this.list = new List<AddExpression>(2);
            this.list.Add(new AddExpression(AddExpression.Signs.Plus, a));
            this.list.Add(new AddExpression(AddExpression.Signs.Plus, b));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Add"/> class.
        /// </summary>
        /// <param name="expressions">Extensible array of <see cref="AddExpression"/> to be contained.</param>
        public Add(params AddExpression[] expressions)
        {
            this.list = new List<AddExpression>();
            foreach (var expression in expressions)
            {
                this.list.Add(expression);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Add"/> class.
        /// </summary>
        /// <param name="list">List of <see cref="AddExpression"/> to be contained.</param>
        public Add(List<AddExpression> list)
        {
            this.list = list;
        }

        /// <inheritdoc />
        public bool IsMonadic
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public int Level
        {
            get
            {
                return 1;
            }
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

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(Add))
            {
                return false;
            }

            Add otherAdd = (Add)other;

            return this.EqualityImplementation(otherAdd);
        }

        public bool Equals(Add other)
        {
            return this.EqualityImplementation(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int finalHash = 17;
                List<int> hashes = new List<int>(this.list.Count);
                foreach (var addExpression in this.list)
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

        /// <inheritdoc />
        public IMaybe<double> Evaluate(double input)
        {
            double sum = 0.0;

            foreach (var expression in this.list)
            {
                var value = expression.Evaluate(input);
                if (!value.HasValue)
                {
                    return new None<double>();
                }

                sum += expression.Sign == AddExpression.Signs.Plus ? (+value.Value) : (-value.Value);
            }

            return new Some<double>(sum);
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            string s = string.Empty;

            foreach (var expression in this.list)
            {
                var value = expression.Print();
                if (!value.HasValue)
                {
                    return new None<string>();
                }

                s += expression.Sign == AddExpression.Signs.Plus ? "+" + value.Value : "-" + value.Value;
            }

            if (s[0] == '+')
            {
                s = s.Remove(0, 1);
            }

            return new Some<string>(s);
        }

        private bool EqualityImplementation(Add other)
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

        /// <summary>
        /// Defines a summand containing a sign and an <see cref="IExpression"/>.
        /// </summary>
        public sealed class AddExpression : IExpression, IEquatable<AddExpression>
        {
            private IExpression expression;

            /// <summary>
            /// Initializes a new instance of the <see cref="AddExpression"/> class.
            /// </summary>
            /// <param name="sign">The sign to be used.</param>
            /// <param name="expression">The expression to be contained.</param>
            public AddExpression(Signs sign, IExpression expression)
            {
                this.expression = expression;
                this.Sign = sign;
            }

            /// <summary>
            /// Enumerates the signs of summands.
            /// </summary>
            public enum Signs
            {
                /// <summary>
                /// Plus sign.
                /// </summary>
                Plus = 1,

                /// <summary>
                /// Minus sign.
                /// </summary>
                Minus = 2
            }

            /// <summary>
            /// Gets the sign of the instance.
            /// </summary>
            public Signs Sign { get; }

            /// <inheritdoc />
            public bool IsMonadic => this.expression.IsMonadic;

            /// <inheritdoc />
            public int Level => this.expression.Level;

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

            /// <inheritdoc />
            public IMaybe<double> Evaluate(double input)
            {
                return this.expression.Evaluate(input);
            }

            /// <inheritdoc />
            public IMaybe<string> Print()
            {
                return this.expression.Print();
            }
        }
    }
}
