using System;
using System.Collections.Generic;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// Defines a dot, which can be hit by an expression and
    /// gives information relevant to its drawing.
    /// </summary>
    public interface IDot
    {
        /// <summary>
        /// Gets the position of the center of the dot in units of the coordinate system.
        /// </summary>
        Tuple<double, double> Position { get; }

        /// <summary>
        /// Gets the radius of the dot in units of the coordinate system.
        /// </summary>
        double Radius { get; }

        /// <summary>
        /// Indicates whether the provided expression hits the dot.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns><c>true</c> if hit, <c>false</c> otherwise.</returns>
        bool IsHit(IExpression expression, List<ListPointLogical> tupleLists);
    }
}
