using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class BaseX : IExpression, IEquatable<BaseX>
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
                return 0;
            }
        }

        public override bool Equals(object other)
        {
            return other.GetType() == typeof(BaseX);
        }

        public bool Equals(BaseX other)
        {
            return true;
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

        public override int GetHashCode()
        {
            return "x".GetHashCode();
        }

        public Maybe<double> Evaluate(double input)
        {
            return new Some<double>(input);
        }

        public Maybe<string> Print()
        {
            return new Some<string>("x");
        }
    }
}
