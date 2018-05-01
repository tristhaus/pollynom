using System;

namespace PollyNomTest.Helper
{
    public class DoubleComparer
    {
        private const double epsilon = 1e-6;

        public static bool IsApproximatelyEqual(double a, double b)
        {
            return (Math.Abs(a - b)) < DoubleComparer.epsilon;
        }
    }
}
