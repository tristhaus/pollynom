using System;

namespace PollyNom.BusinessLogic.Expressions
{
    /// <summary>
    /// Implements an invalid expression following the Null Object Pattern.
    /// </summary>
    public sealed class InvalidExpression : IExpression, IEquatable<InvalidExpression>
    {
        /// <inheritdoc />
        public bool IsMonadic
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public int Level
        {
            get
            {
                return -1;
            }
        }

        public override bool Equals(object other)
        {
            return other.GetType() == typeof(InvalidExpression);
        }

        public bool Equals(InvalidExpression other)
        {
            return true;
        }

        public static bool operator ==(InvalidExpression x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(InvalidExpression x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(InvalidExpression x, InvalidExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(InvalidExpression x, InvalidExpression y)
        {
            return !(x.Equals(y));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return "invalid".GetHashCode();
            }
        }

        /// <inheritdoc />
        public IMaybe<double> Evaluate(double input)
        {
            return new None<double>();
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            return new None<string>();
        }
    }
}
