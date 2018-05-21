using System;

namespace PollyNom.BusinessLogic.Dots
{
    public class GoodDot : IDot
    {
        private const double radius = 0.25;
        private readonly double x;
        private readonly double y;

        public GoodDot(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Tuple<double, double> Position
        {
            get
            {
                return new Tuple<double, double>(this.x, this.y);
            }
        }

        public double Radius
        {
            get
            {
                return GoodDot.radius;
            }
        }

        public bool IsHit(IExpression expression)
        {
            return false;
        }
    }
}
