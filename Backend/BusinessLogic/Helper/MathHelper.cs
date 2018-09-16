namespace Backend.BusinessLogic.Helper
{
    /// <summary>
    /// Provides math functions.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Calculates the square of the Euclidean distance between
        /// two points provided as single coordinates.
        /// </summary>
        /// <param name="x1">X-coordinate of the first point.</param>
        /// <param name="y1">Y-coordinate of the first point.</param>
        /// <param name="x2">X-coordinate of the second point.</param>
        /// <param name="y2">Y-coordinate of the second point.</param>
        /// <returns>The squared distance.</returns>
        public static double SquareDistance(double x1, double y1, double x2, double y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }
    }
}
