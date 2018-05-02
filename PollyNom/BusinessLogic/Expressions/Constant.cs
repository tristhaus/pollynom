using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Constant : IExpression, IEquatable<Constant>
    {
        private double a;

        public Constant(double a)
        {
            this.a = a;
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
                return 0;
            }
        }

        public override bool Equals(object other)
        {
            if(other.GetType() != typeof(Constant))
            {
                return false;
            }
            Constant otherConstant = (Constant)other;
            return Math.Abs(a - otherConstant.a) < 10e-10;
        }

        public bool Equals(Constant other)
        {
            return Math.Abs(a - other.a) < 10e-10;
        }

        public static bool operator ==(Constant x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Constant x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(Constant x, Constant y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Constant x, Constant y)
        {
            return !(x.Equals(y));
        }

        public override int GetHashCode()
        {
            return a.GetHashCode();
        }

        public Maybe<double> Evaluate(double input)
        {
            return new Some<double>(a);
        }

        public Maybe<string> Print()
        {
            return new Some<string>($"{a}");
        }
    }
}
