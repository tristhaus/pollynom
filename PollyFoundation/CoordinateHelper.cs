using System;
using System.Windows;

namespace PollyFoundation
{
    public class CoordinateHelper
    {
        private double canvasWidth;
        private double canvasHeight;
        private double fullInterval;
        private double margin;

        private double xAnchor;
        private double yAnchor;
        private double xFactor;
        private double yFactor;

        public CoordinateHelper(double canvasWidth, double canvasHeight, double margin, double fullInterval)
        {
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            this.fullInterval = fullInterval;
            this.margin = margin;

            this.Initialize();
        }

        public Point ConvertCoordinates(double x, double y)
        {
            double transformedX = this.xAnchor + this.xFactor * x;
            double transformedY = this.yAnchor + this.yFactor * y;
            return new Point(transformedX, transformedY);
        }

        public double ConvertXLength(double input)
        {
            return Math.Abs(input * this.xFactor);
        }

        private void Initialize()
        {
            this.xAnchor = this.canvasWidth / 2.0;
            this.yAnchor = this.canvasHeight / 2.0;

            double squareLength = Math.Min(this.canvasWidth, this.canvasHeight) - 2.0 * this.margin;

            this.xFactor = +squareLength / this.fullInterval;
            this.yFactor = -this.xFactor;
        }
    }
}
