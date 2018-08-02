using System;

namespace PollyNomTest.Helper
{
    /// <summary>
    /// Allows to compare two doubles for approximate equality.
    /// </summary>
    internal static class DoubleEquality
    {
        /// <summary>
        /// Threshold below which the absolute difference must be for equality.
        /// </summary>
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Compare the provided doubles for approximate equality.
        /// </summary>
        /// <param name="a">First double to be compared.</param>
        /// <param name="b">Second double to be compared.</param>
        /// <returns><c>true</c> if the doubles are close enough to each other.</returns>
        public static bool IsApproximatelyEqual(double a, double b)
        {
            return (Math.Abs(a - b)) < DoubleEquality.Epsilon;
        }
    }
}
