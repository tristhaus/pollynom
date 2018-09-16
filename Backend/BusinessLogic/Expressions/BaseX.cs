using System;

namespace Backend.BusinessLogic.Expressions
{
    /// <summary>
    /// Implements a variable which will accept a value on evaluation.
    /// </summary>
    public sealed class BaseX : IExpression, IEquatable<BaseX>
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
                return 0;
            }
        }

        public static bool operator ==(BaseX x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(BaseX x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(BaseX x, BaseX y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(BaseX x, BaseX y)
        {
            return !(x.Equals(y));
        }

        public override bool Equals(object other)
        {
            return other.GetType() == typeof(BaseX);
        }

        public bool Equals(BaseX other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return "x".GetHashCode();
            }
        }

        /// <inheritdoc />
        public IMaybe<double> Evaluate(double input)
        {
            return new Some<double>(input);
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            return new Some<string>("x");
        }
    }
}
