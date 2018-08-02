using System;

namespace PollyNom.BusinessLogic.Expressions
{
    /// <summary>
    /// Implements a power using a base and exponent.
    /// </summary>
    public sealed class Power : IExpression, IEquatable<Power>
    {
        private IExpression @base;
        private IExpression exponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Power"/> class.
        /// </summary>
        /// <param name="base">The base of the <see cref="Power"/>expression.</param>
        /// <param name="exponent">The exponent to which the <see cref="base"/> is raised.</param>
        public Power(IExpression @base, IExpression exponent)
        {
            this.@base = @base;
            this.exponent = exponent;
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
                return 3;
            }
        }

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(Power))
            {
                return false;
            }

            Power otherPower = (Power)other;

            return this.EqualityImplementation(otherPower);
        }

        public bool Equals(Power other)
        {
            return this.EqualityImplementation(other);
        }

        public static bool operator ==(Power x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Power x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(Power x, Power y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Power x, Power y)
        {
            return !(x.Equals(y));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                    hash = hash * 23 + this.@base.GetHashCode();
                    hash = hash * 23 + this.exponent.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc />
        public IMaybe<double> Evaluate(double input)
        {
            var aValue = this.@base.Evaluate(input);
            var bValue = this.exponent.Evaluate(input);
            if (!aValue.HasValue || !bValue.HasValue)
            {
                return new None<double>();
            }

            double value = Math.Pow(aValue.Value, bValue.Value);
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                return new None<double>();
            }

            return new Some<double>(value);
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            var aValue = this.@base.Print();
            var bValue = this.exponent.Print();
            if (!aValue.HasValue || !bValue.HasValue)
            {
                return new None<string>();
            }

            var aDecorated = aValue.Value;
            var bDecorated = bValue.Value;
            if (!this.@base.IsMonadic)
            {
                aDecorated = "(" + aDecorated + ")";
            }

            if (!this.exponent.IsMonadic)
            {
                bDecorated = "(" + bDecorated + ")";
            }

            return new Some<string>(aDecorated + "^" + bDecorated);
        }

        private bool EqualityImplementation(Power other)
        {
            return this.@base.Equals(other.@base) && this.exponent.Equals(other.exponent);
        }
    }
}
