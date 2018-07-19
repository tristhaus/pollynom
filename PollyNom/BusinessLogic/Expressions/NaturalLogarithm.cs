using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class NaturalLogarithm : IExpression, IEquatable<NaturalLogarithm>
    {
        IExpression containedExpression;

        public NaturalLogarithm(IExpression containedExpression)
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
            if (other.GetType() != typeof(NaturalLogarithm))
            {
                return false;
            }
            NaturalLogarithm otherExponential = (NaturalLogarithm)other;

            return this.equalityImplementation(otherExponential);
        }

        public bool Equals(NaturalLogarithm other)
        {
            return this.equalityImplementation(other);
        }

        public static bool operator ==(NaturalLogarithm x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(NaturalLogarithm x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(NaturalLogarithm x, NaturalLogarithm y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(NaturalLogarithm x, NaturalLogarithm y)
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
            if (!value.HasValue || (value.HasValue && value.Value <= 0.0))
            {
                return new None<double>();
            }

            return new Some<double>(Math.Log(value.Value));
        }

        public Maybe<string> Print()
        {
            var value = containedExpression.Print();
            if (!value.HasValue)
            {
                return new None<string>();
            }

            return new Some<string>($"ln({value.Value})");
        }

        private bool equalityImplementation(NaturalLogarithm other)
        {
            return containedExpression.Equals(other.containedExpression);
        }
    }
}
