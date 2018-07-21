using System;

namespace PollyNom.BusinessLogic.Expressions
{
    /// <summary>
    /// Implements a constant expression.
    /// </summary>
    public sealed class Constant : IExpression, IEquatable<Constant>
    {
        private double a;

        /// <summary>
        /// Creates a new instance of the <see cref="Constant"/> class.
        /// </summary>
        public Constant(double a)
        {
            this.a = a;
        }

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
            unchecked
            {
                return a.GetHashCode();
            }
        }

        /// <inheritdoc />
        public Maybe<double> Evaluate(double input)
        {
            return new Some<double>(a);
        }

        /// <inheritdoc />
        public Maybe<string> Print()
        {
            return new Some<string>($"{a}");
        }
    }
}
