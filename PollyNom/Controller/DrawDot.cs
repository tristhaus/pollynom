using System;

namespace PollyNom.Controller
{
    public class DrawDot : IDrawDot
    {
        private readonly double xCoordinate;
        private readonly double yCoordinate;
        private readonly double radius;
        private readonly DrawDotKind kind;

        public DrawDot(double xCoordinate, double yCoordinate, double radius, DrawDotKind kind)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.radius = radius;
            this.kind = kind;

            this.IsHit = false;
        }

        public Tuple<double, double> Position
        {
            get
            {
                return new Tuple<double, double>(this.xCoordinate, this.yCoordinate);
            }
        }

        public double Radius
        {
            get
            {
                return this.radius;
            }
        }

        public bool IsHit { get; set; }

        public DrawDotKind Kind
        {
            get
            {
                return this.kind;
            }
        }
    }
}
