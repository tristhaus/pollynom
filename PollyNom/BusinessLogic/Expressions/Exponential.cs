using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Exponential : IExpression, IEquatable<Exponential>
    {
        IExpression containedExpression;

        public Exponential(IExpression containedExpression)
        {
            this.containedExpression = containedExpression;
        }

        public bool IsMonadic
        {
            get
            {
                return true;
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
            if (other.GetType() != typeof(Exponential))
            {
                return false;
            }
            Exponential otherExponential = (Exponential)other;

            return this.equalityImplementation(otherExponential);
        }

        public bool Equals(Exponential other)
        {
            return this.equalityImplementation(other);
        }

        public static bool operator ==(Exponential x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Exponential x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(Exponential x, Exponential y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Exponential x, Exponential y)
        {
            return !(x.Equals(y));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + containedExpression.GetHashCode();
                return hash;
            }
        }

        public Maybe<double> Evaluate(double input)
        {
            var value = containedExpression.Evaluate(input);
            if(!value.HasValue)
            {
                return new None<double>();
            }

            return new Some<double>(Math.Exp(value.Value));
        }

        public Maybe<string> Print()
        {
            var value = containedExpression.Print();
            if (!value.HasValue)
            {
                return new None<string>();
            }

            return new Some<string>($"exp({value.Value})");
        }

        private bool equalityImplementation(Exponential other)
        {
            return containedExpression.Equals(other.containedExpression);
        }
    }
}
