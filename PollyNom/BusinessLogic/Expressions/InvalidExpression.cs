using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class InvalidExpression : IExpression, IEquatable<InvalidExpression>
    {
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
            return "invalid".GetHashCode();
        }

        public Maybe<double> Evaluate(double input)
        {
            return new None<double>();
        }

        public Maybe<string> Print()
        {
            return new None<string>();
        }
    }
}
