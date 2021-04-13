/*
 * This file is part of PollyNom.
 * 
 * PollyNom is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * PollyNom is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with PollyNom.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

﻿using System;
using System.Windows;

namespace PollyFoundation
{
    public class CoordinateHelper
    {
        private readonly double canvasWidth;
        private readonly double canvasHeight;
        private readonly double fullInterval;
        private readonly double margin;

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
