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
    /// Implements a variable which will accept a value on evaluation.
    /// </summary>
    public sealed class BaseX : IExpression, IEquatable<BaseX>
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
                return 0;
            }
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

        public override bool Equals(object other)
        {
            return other.GetType() == typeof(BaseX);
        }

        public bool Equals(BaseX other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return "x".GetHashCode();
            }
        }

        /// <inheritdoc />
        public IMaybe<double> Evaluate(double input)
        {
            return new Some<double>(input);
        }

        /// <inheritdoc />
        public IMaybe<string> Print()
        {
            return new Some<string>("x");
        }
    }
}
