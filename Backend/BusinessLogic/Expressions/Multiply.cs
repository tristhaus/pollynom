/*
 * This file is part of PollyNom.
 * 
 * PollyNom is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * PollyNom is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with PollyNom.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.BusinessLogic.Expressions
{
    /// <summary>
    /// Implements a product over an arbritrary, fixed list of factors.
    /// </summary>
    public sealed class Multiply : IExpression, IEquatable<Multiply>
    {
        private readonly List<MultiplyExpression> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="Multiply"/> class.
        /// </summary>
        /// <param name="a">First factor of the resulting expression.</param>
        /// <param name="b">Second and last factor of the resulting expression.</param>
        public Multiply(IExpression a, IExpression b)
        {
            this.list = new List<MultiplyExpression>(2);
            this.list.Add(new MultiplyExpression(MultiplyExpression.Signs.Multiply, a));
            this.list.Add(new MultiplyExpression(MultiplyExpression.Signs.Multiply, b));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Multiply"/> class.
        /// </summary>
        /// <param name="expressions">Extensible array of <see cref="MultiplyExpression"/> to be contained.</param>
        public Multiply(params MultiplyExpression[] expressions)
        {
            this.list = new List<MultiplyExpression>();
            foreach (var expression in expressions)
            {
                this.list.Add(expression);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Multiply"/> class.
        /// </summary>
        /// <param name="list">List of <see cref="MultiplyExpression"/> to be contained.</param>
        public Multiply(List<MultiplyExpression> list)
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
                return 2;
            }
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

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(Multiply))
            {
                return false;
            }

            Multiply otherMultiply = (Multiply)other;

            return this.EqualityImplementation(otherMultiply);
        }

        public bool Equals(Multiply other)
        {
            return this.EqualityImplementation(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int finalHash = 17;
                List<int> hashes = new List<int>(this.list.Count);
                foreach (var multiplyExpression in this.list)
                {
                    hashes.Add(multiplyExpression.GetHashCode());
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
            double product = 1.0;

            foreach (var expression in this.list)
            {
                var value = expression.Evaluate(input);
                if (!value.HasValue || (expression.Sign == MultiplyExpression.Signs.Divide && Math.Abs(value.Value) < 10e-10))
                {
                    return new None<double>();
                }

                var finalValue = expression.Sign == MultiplyExpression.Signs.Multiply ? value.Value : (1.0 / value.Value);
                if (double.IsInfinity(finalValue) || double.IsNaN(finalValue))
                {
                    return new None<double>();
                }

                product *= finalValue;
            }

            return new Some<double>(product);
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            string s = "1";

            foreach (var expression in this.list)
            {
                var value = expression.Print();
                if (!value.HasValue)
                {
                    return new None<string>();
                }

                var decoratedValue = value.Value;
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

        private bool EqualityImplementation(Multiply other)
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

        /// <summary>
        /// Defines a factor containing a "sign" and an <see cref="IExpression"/>.
        /// </summary>
        public sealed class MultiplyExpression : IExpression, IEquatable<MultiplyExpression>
        {
            private readonly IExpression expression;

            /// <summary>
            /// Initializes a new instance of the <see cref="MultiplyExpression"/> class.
            /// </summary>
            /// <param name="sign">The "sign" to be used.</param>
            /// <param name="expression">The expression to be contained.</param>
            public MultiplyExpression(Signs sign, IExpression expression)
            {
                this.expression = expression;
                this.Sign = sign;
            }

            /// <summary>
            /// Enumerates the "signs" of factors.
            /// </summary>
            public enum Signs
            {
                /// <summary>
                /// Makes the expression work in a multiplicative fashion.
                /// </summary>
                Multiply = 3,

                /// <summary>
                /// Makes the expression work in a dividing fashion.
                /// </summary>
                Divide = 4
            }

            /// <summary>
            /// Gets the "sign" of the instance.
            /// </summary>
            public Signs Sign { get; }

            /// <inheritdoc />
            public bool IsMonadic => this.expression.IsMonadic;

            /// <inheritdoc />
            public int Level => this.expression.Level;

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
