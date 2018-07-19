using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Power : IExpression, IEquatable<Power>
    {
        private IExpression basis;
        private IExpression exponent;

        public Power(IExpression a, IExpression b)
        {
            this.basis = a;
            this.exponent = b;
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

            return this.equalityImplementation(otherPower);
        }

        public bool Equals(Power other)
        {
            return this.equalityImplementation(other);
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
                    hash = hash * 23 + basis.GetHashCode();
                    hash = hash * 23 + exponent.GetHashCode();
                return hash;
            }
        }

        public Maybe<double> Evaluate(double input)
        {
            var aValue = this.basis.Evaluate(input);
            var bValue = this.exponent.Evaluate(input);
            if (!aValue.HasValue|| !bValue.HasValue)
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

        public Maybe<string> Print()
        {
            var aValue = this.basis.Print();
            var bValue = this.exponent.Print();
            if (!aValue.HasValue|| !bValue.HasValue)
            {
                return new None<string>();
            }

            var aDecorated = aValue.Value;
            var bDecorated = bValue.Value;
            if (!basis.IsMonadic)
            {
                aDecorated = "(" + aDecorated + ")";
            }
            if (!exponent.IsMonadic)
            {
                bDecorated = "(" + bDecorated + ")";
            }

            return new Some<string>(aDecorated + "^" + bDecorated);
        }

        private bool equalityImplementation(Power other)
        {
            return basis.Equals(other.basis) && exponent.Equals(other.exponent);
        }
    }
}
