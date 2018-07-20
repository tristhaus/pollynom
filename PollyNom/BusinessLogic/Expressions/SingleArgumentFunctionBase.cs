using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public abstract class SingleArgumentFunctionBase<T> : IExpression, IEquatable<SingleArgumentFunctionBase<T>>
    {
        private IExpression containedExpression;

        /// <summary>
        /// Creates a new instance of the <see cref="SingleArgumentFunctionBase{T}"/> class.
        /// </summary>
        /// <param name="containedExpression">The single argument of the function.</param>
        protected SingleArgumentFunctionBase(IExpression containedExpression)
        {
            this.containedExpression = containedExpression;
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
                return 3;
            }
        }

        public override bool Equals(object other)
        {
            if (other.GetType() != this.GetType())
            {
                return false;
            }
            SingleArgumentFunctionBase<T> otherFunction = (SingleArgumentFunctionBase<T>)other;

            return this.equalityImplementation(otherFunction);
        }

        public bool Equals(SingleArgumentFunctionBase<T> other)
        {
            return this.equalityImplementation(other);
        }

        public static bool operator ==(SingleArgumentFunctionBase<T> x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(SingleArgumentFunctionBase<T> x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(SingleArgumentFunctionBase<T> x, SingleArgumentFunctionBase<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(SingleArgumentFunctionBase<T> x, SingleArgumentFunctionBase<T> y)
        {
            return !(x.Equals(y));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + containedExpression.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Must be overridden to validate the argument to prevent 
        /// illegal input to <see cref="FunctionFunc"/>.
        /// </summary>
        protected abstract bool ArgumentIsValid(double argument);

        /// <summary>
        /// Must be overridden by a <see cref="Math"/> or similar function.
        /// </summary>
        protected abstract Func<double, double> FunctionFunc { get; }

        /// <inheritdoc />
        public Maybe<double> Evaluate(double input)
        {
            var value = containedExpression.Evaluate(input);
            if (!value.HasValue || !this.ArgumentIsValid(value.Value))
            {
                return new None<double>();
            }

            return new Some<double>(this.FunctionFunc(value.Value));
        }

        /// <summary>
        /// Must be overridden by the textual representation of the function.
        /// </summary>
        protected abstract string FunctionSymbol { get; }

        /// <inheritdoc />
        public Maybe<string> Print()
        {
            var value = containedExpression.Print();
            if (!value.HasValue)
            {
                return new None<string>();
            }

            return new Some<string>($"{this.FunctionSymbol}({value.Value})");
        }

        private bool equalityImplementation(SingleArgumentFunctionBase<T> other)
        {
            return containedExpression.Equals(other.containedExpression);
        }
    }
}
