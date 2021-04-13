/*
 * This file is part of PollyNom.
 * 
 * PollyNom is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * PollyNom is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with PollyNom.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

﻿using System;

namespace Backend.BusinessLogic.Expressions
{
    /// <summary>
    /// Base class for implementing functions that accept single arguments
    /// as <see cref="IExpression"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type that implements this base, as needed for equality determination
    /// using the <see cref="IEquatable{U}"/> interface.
    /// </typeparam>
    public abstract class SingleArgumentFunctionBase<T> : IExpression, IEquatable<SingleArgumentFunctionBase<T>>
    {
        private readonly IExpression containedExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleArgumentFunctionBase{T}"/> class.
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

        /// <summary>
        /// Gets a <see cref="Func{T, TResult}"/> object that represents the function.
        /// Must be overridden by a <see cref="Math"/> or similar function.
        /// </summary>
        protected abstract Func<double, double> FunctionFunc { get; }

        /// <summary>
        /// Gets a string representation of the function.
        /// Must be overridden.
        /// </summary>
        protected abstract string FunctionSymbol { get; }

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

        public override bool Equals(object other)
        {
            if (other.GetType() != this.GetType())
            {
                return false;
            }

            SingleArgumentFunctionBase<T> otherFunction = (SingleArgumentFunctionBase<T>)other;

            return this.EqualityImplementation(otherFunction);
        }

        public bool Equals(SingleArgumentFunctionBase<T> other)
        {
            return this.EqualityImplementation(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.containedExpression.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc />
        public IMaybe<double> Evaluate(double input)
        {
            var value = this.containedExpression.Evaluate(input);
            if (!value.HasValue || !this.ArgumentIsValid(value.Value))
            {
                return new None<double>();
            }

            return new Some<double>(this.FunctionFunc(value.Value));
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            var value = this.containedExpression.Print();
            if (!value.HasValue)
            {
                return new None<string>();
            }

            return new Some<string>($"{this.FunctionSymbol}({value.Value})");
        }

        /// <summary>
        /// Must be overridden to validate the argument to prevent
        /// illegal input to <see cref="FunctionFunc"/>.
        /// </summary>
        /// <param name="argument">The numerical value to be checked for validity.</param>
        /// <returns>A flag indicating whether the argument is valid for this function.</returns>
        protected abstract bool ArgumentIsValid(double argument);

        private bool EqualityImplementation(SingleArgumentFunctionBase<T> other)
        {
            return this.containedExpression.Equals(other.containedExpression);
        }
    }
}
