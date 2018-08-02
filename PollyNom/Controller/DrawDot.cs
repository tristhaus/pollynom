using System;

namespace PollyNom.Controller
{
    /// <summary>
    /// A drawable dot.
    /// </summary>
    public class DrawDot : IDrawDot
    {
        private readonly double xCoordinate;
        private readonly double yCoordinate;
        private readonly double radius;
        private readonly DrawDotKind kind;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawDot"/> class.
        /// </summary>
        /// <param name="xCoordinate">X-coordinate of the dot.</param>
        /// <param name="yCoordinate">Y-coordinate of the dot.</param>
        /// <param name="radius">Radius of the dot.</param>
        /// <param name="kind">The kind of the dot.</param>
        public DrawDot(double xCoordinate, double yCoordinate, double radius, DrawDotKind kind)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.radius = radius;
            this.kind = kind;

            this.IsHit = false;
        }

        /// <summary>
        /// Gets the position of the dot as an x,y tuple.
        /// </summary>
        public Tuple<double, double> Position
        {
            get
            {
                return new Tuple<double, double>(this.xCoordinate, this.yCoordinate);
            }
        }

        /// <summary>
        /// Gets the radius of the dot.
        /// </summary>
        public double Radius
        {
            get
            {
                return this.radius;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dot has been hit.
        /// </summary>
        public bool IsHit { get; set; }

        /// <summary>
        /// Gets the kind of the dot.
        /// </summary>
        public DrawDotKind Kind
        {
            get
            {
                return this.kind;
            }
        }
    }
}
