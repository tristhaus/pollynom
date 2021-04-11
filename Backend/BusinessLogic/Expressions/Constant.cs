using System;

namespace Backend.BusinessLogic.Expressions
{
    /// <summary>
    /// Implements a constant expression.
    /// </summary>
    public sealed class Constant : IExpression, IEquatable<Constant>
    {
        private static readonly IFormatProvider formatProvider = new System.Globalization.CultureInfo("en-US") as IFormatProvider;
        private readonly double a;

        /// <summary>
        /// Initializes a new instance of the <see cref="Constant"/> class.
        /// </summary>
        /// <param name="a">The numerical value of the constant.</param>
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
            return !x.Equals(y);
        }

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(Constant))
            {
                return false;
            }

            Constant otherConstant = (Constant)other;
            return Math.Abs(this.a - otherConstant.a) < 10e-10;
        }

        public bool Equals(Constant other)
        {
            return Math.Abs(this.a - other.a) < 10e-10;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return this.a.GetHashCode();
            }
        }

        /// <inheritdoc />
        public IMaybe<double> Evaluate(double input)
        {
            return new Some<double>(this.a);
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            return new Some<string>($"{this.a.ToString(formatProvider)}");
        }
    }
}
