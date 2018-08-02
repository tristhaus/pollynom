using System;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// Defines the Maybe design pattern.
    /// </summary>
    /// <typeparam name="T">The type of the value that can be held.</typeparam>
    public interface IMaybe<T>
    {
        /// <summary>
        /// Gets a value indicating whether a value is held by the instance.
        /// </summary>
        /// <returns><c>true</c> if a value exists.</returns>
        bool HasValue { get; }

        /// <summary>
        /// Gets the value contained.
        /// </summary>
        T Value { get; }
    }

    /// <summary>
    /// Implementation of a value that actually exists.
    /// </summary>
    /// <typeparam name="T">The type of the value that can be held.</typeparam>
    public class Some<T> : IMaybe<T>
    {
        private readonly T t;

        /// <summary>
        /// Initializes a new instance of the <see cref="Some{T}"/> class,
        /// given a value to be held.
        /// </summary>
        /// <param name="t">The value to be held by this instance.</param>
        public Some(T t)
        {
            this.t = t;
        }

        /// <inheritdoc />
        public bool HasValue
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public T Value
        {
            get
            {
                return this.t;
            }
        }
    }

    /// <summary>
    /// Implementation of a non-existing value.
    /// </summary>
    /// <typeparam name="T">The type of the value that could have been held.</typeparam>
    public class None<T> : IMaybe<T>
    {
        /// <inheritdoc />
        public bool HasValue
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets an exception.
        /// </summary>
        public T Value
        {
            get
            {
                throw new NotSupportedException("cannot provide value");
            }
        }
    }
}
