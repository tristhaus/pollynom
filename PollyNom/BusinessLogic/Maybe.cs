using System;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// Defines the Maybe design pattern.
    /// </summary>
    /// <typeparam name="T">The type of the value that can be held.</typeparam>
    public interface Maybe<T>
    {
        /// <summary>
        /// Indicates whether a value is held by the instance.
        /// </summary>
        /// <returns><c>true</c> if a value exists.</returns>
        bool HasValue { get; }

        /// <summary>
        /// Provides access to the value.
        /// </summary>
        /// <returns>The value held, if any.</returns>
        T Value { get; }
    }

    /// <summary>
    /// Implementation of a value that actually exists.
    /// </summary>
    /// <typeparam name="T">The type of the value that can be held.</typeparam>
    public class Some<T> : Maybe<T>
    {
        private readonly T t;

        /// <summary>
        /// Creates a new instance of the <see cref="Some"/> class,
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
                return t;
            }
        }
    }

    /// <summary>
    /// Implementation of a non-existing value.
    /// </summary>
    /// <typeparam name="T">The type of the value that could have been held.</typeparam>
    public class None<T> : Maybe<T>
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
        /// Does not provide access, but throws an exception on access.
        /// </summary>
        /// <returns>Nothing, will throw exception instead.</returns>
        public T Value
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
