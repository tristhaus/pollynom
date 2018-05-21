using System;

namespace PollyNom.BusinessLogic
{
    public interface IDot
    {
        /// <summary>
        /// Gets the position of the center of the dot in units of the coordinate system.
        /// </summary>
        Tuple<double, double> Position { get; }

        /// <summary>
        /// Gets the radius of the dot in units of the coordinate system.
        /// </summary>
        double radius { get; }

        /// <summary>
        /// Indicates whether the provided expression hits the dot.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns><c>true</c> if hit, <c>false</c> otherwise.</returns>
        bool IsHit(IExpression expression);
    }
}
