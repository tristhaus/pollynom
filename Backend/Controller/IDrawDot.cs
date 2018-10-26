using System;
using Persistence.Models;

namespace Backend.Controller
{
    /// <summary>
    /// Defines a dot, which can be hit by an expression and
    /// gives information relevant to its drawing.
    /// </summary>
    public interface IDrawDot
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
        /// Gets a value indicating whether the dot was hit.
        /// </summary>
        /// <returns><c>true</c> if hit, <c>false</c> otherwise.</returns>
        bool IsHit { get; }

        /// <summary>
        ///  Gets the kind of the dot, as defined in <see cref="DotKind"/>.
        /// </summary>
        DotKind Kind { get; }
    }
}