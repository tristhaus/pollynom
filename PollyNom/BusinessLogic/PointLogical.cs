using System;

namespace PollyNom
{
    public class PointLogical : IComparable<PointLogical>
    {
        public readonly double X;
        public readonly double Y;

        public PointLogical(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public int CompareTo(PointLogical other)
        {
            return X.CompareTo(other.X);
        }
    }
}
