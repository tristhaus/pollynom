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

﻿namespace Backend.BusinessLogic
{
    /// <summary>
    /// Defines a mathematical expression, i.e. a function of x.
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// The precedence level of the underlying operation.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Indicates whether the expression is monadic.
        /// </summary>
        bool IsMonadic { get; }

        /// <summary>
        /// Evaluates the expression at a given x-coordinate.
        /// </summary>
        /// <param name="input">The x-coordinate at which the expression shall be evaluated.</param>
        /// <returns>A <see cref="IMaybe{double}"/> representing the result of the evaluation.</returns>
        IMaybe<double> Evaluate(double input);

        /// <summary>
        /// Prints the expression as a human-readable and machine-parseable string.
        /// </summary>
        /// <returns>A <see cref="IMaybe{double}"/> representing the printed string.</returns>
        IMaybe<string> Print();
    }
}
