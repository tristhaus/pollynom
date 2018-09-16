using System;
using System.Collections.Generic;

namespace Backend.BusinessLogic
{
    /// <summary>
    /// Defines a dot, which can be hit by an expression and
    /// gives information relevant to its drawing.
    /// </summary>
    public interface IDot
    {
        /// <summary>
        /// Gets the position of the center of the dot in logical business units.
        /// </summary>
        Tuple<double, double> Position { get; }

        /// <summary>
        /// Gets the radius of the dot in logical business units.
        /// </summary>
        double Radius { get; }

        /// <summary>
        /// Indicates whether the provided expression (also represented as a series of pre-evaluated points) hits the dot.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <param name="logicalPointLists">Lists of pre-evaluated points</param>
        /// <returns><c>true</c> if hit, <c>false</c> otherwise.</returns>
        bool IsHit(IExpression expression, List<ListPointLogical> logicalPointLists);
    }
}
