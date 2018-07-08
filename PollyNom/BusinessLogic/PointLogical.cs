using System;

namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// Represents a point using implied logical business units.
    /// </summary>
    public class PointLogical : IComparable<PointLogical>
    {
        /// <summary>
        /// The x-coordinate in implied logical business units.
        /// </summary>
        public readonly double X;

        /// <summary>
        /// The y-coordinate in implied logical business units.
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// Creates a new instance of the <see cref="PointLogical"/> class from the coordinates given.
        /// </summary>
        /// <param name="x">X-coordinate of the point in implied logical business units.</param>
        /// <param name="y">Y-coordinate of the point in implied logical business units.</param>
        public PointLogical(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <inheritdoc />
        public int CompareTo(PointLogical other)
        {
            return X.CompareTo(other.X);
        }
    }
}
