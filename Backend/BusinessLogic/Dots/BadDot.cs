namespace Backend.BusinessLogic.Dots
{
    /// <summary>
    /// Represents a dot that should not be hit.
    /// </summary>
    public class BadDot : DotBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadDot"/> class from the coordinates given.
        /// </summary>
        /// <param name="x">X-coordinate of the dot in implied logical business units.</param>
        /// <param name="y">Y-coordinate of the dot in implied logical business units.</param>
        public BadDot(double x, double y)
            : base(x, y)
        {
        }

        /// <inheritdoc />
        public override DotKind Kind => DotKind.Bad;
    }
}
