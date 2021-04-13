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
    /// Implements an invalid expression following the Null Object Pattern.
    /// </summary>
    public sealed class InvalidExpression : IExpression, IEquatable<InvalidExpression>
    {
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
                return -1;
            }
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

        public override bool Equals(object other)
        {
            return other.GetType() == typeof(InvalidExpression);
        }

        public bool Equals(InvalidExpression other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return "invalid".GetHashCode();
            }
        }

        /// <inheritdoc />
        public IMaybe<double> Evaluate(double input)
        {
            return new None<double>();
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            return new None<string>();
        }
    }
}
