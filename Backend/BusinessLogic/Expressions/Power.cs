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
    /// Implements a power using a base and exponent.
    /// </summary>
    public sealed class Power : IExpression, IEquatable<Power>
    {
        private readonly IExpression @base;
        private readonly IExpression exponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Power"/> class.
        /// </summary>
        /// <param name="base">The base of the <see cref="Power"/>expression.</param>
        /// <param name="exponent">The exponent to which the <see cref="base"/> is raised.</param>
        public Power(IExpression @base, IExpression exponent)
        {
            this.@base = @base;
            this.exponent = exponent;
        }

        /// <inheritdoc />
        public bool IsMonadic
        {
            get
            {
                return false;
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

        public static bool operator ==(Power x, IExpression y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Power x, IExpression y)
        {
            return !(x.Equals(y));
        }

        public static bool operator ==(Power x, Power y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Power x, Power y)
        {
            return !(x.Equals(y));
        }

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(Power))
            {
                return false;
            }

            Power otherPower = (Power)other;

            return this.EqualityImplementation(otherPower);
        }

        public bool Equals(Power other)
        {
            return this.EqualityImplementation(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                    hash = hash * 23 + this.@base.GetHashCode();
                    hash = hash * 23 + this.exponent.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc />
        public IMaybe<double> Evaluate(double input)
        {
            var aValue = this.@base.Evaluate(input);
            var bValue = this.exponent.Evaluate(input);
            if (!aValue.HasValue || !bValue.HasValue)
            {
                return new None<double>();
            }

            double value = Math.Pow(aValue.Value, bValue.Value);
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                return new None<double>();
            }

            return new Some<double>(value);
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            var aValue = this.@base.Print();
            var bValue = this.exponent.Print();
            if (!aValue.HasValue || !bValue.HasValue)
            {
                return new None<string>();
            }

            var aDecorated = aValue.Value;
            var bDecorated = bValue.Value;
            if (!this.@base.IsMonadic)
            {
                aDecorated = "(" + aDecorated + ")";
            }

            if (!this.exponent.IsMonadic)
            {
                bDecorated = "(" + bDecorated + ")";
            }

            return new Some<string>(aDecorated + "^" + bDecorated);
        }

        private bool EqualityImplementation(Power other)
        {
            return this.@base.Equals(other.@base) && this.exponent.Equals(other.exponent);
        }
    }
}
