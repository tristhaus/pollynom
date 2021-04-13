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

﻿using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyFoundation;
using TestInfrastructure;

namespace PollyNomTest.PollyFoundation
{
    /// <summary>
    /// Collects tests related to coordinate transformation for the WPF <see cref="PollyFoundation"/>.
    /// </summary>
    [TestClass]
    public class CoordinateTransformationTest
    {
        /// <summary>
        /// Simple tests on a square canvas.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void SquareSimple()
        {
            // Arrange
            double canvasDimension = 100.0;
            double margin = 0.0;
            double fullInterval = 20.0;
            var coordinateHelper = new CoordinateHelper(canvasDimension, canvasDimension, margin, fullInterval);

            // Act
            Point origin = coordinateHelper.ConvertCoordinates(0.0, 0.0);
            Point topLeft = coordinateHelper.ConvertCoordinates(-10, 10);
            Point bottomLeft = coordinateHelper.ConvertCoordinates(-10, -10);
            Point topRight = coordinateHelper.ConvertCoordinates(10, 10);
            Point bottomRight = coordinateHelper.ConvertCoordinates(10, -10);

            // Assert
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.X, 50.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.Y, 50.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.X, 0.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.Y, 0.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.X, 0.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.Y, 100.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.X, 100.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.Y, 0.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.X, 100.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.Y, 100.0));
        }

        /// <summary>
        /// Simple tests on a landscape rectangle.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void RectangleLandscapeSimple()
        {
            // Arrange
            double canvasWidth = 120.0;
            double canvasHeight = 100.0;
            double margin = 0.0;
            double fullInterval = 20.0;
            var coordinateHelper = new CoordinateHelper(canvasWidth, canvasHeight, margin, fullInterval);

            // Act
            Point origin = coordinateHelper.ConvertCoordinates(0.0, 0.0);
            Point topLeft = coordinateHelper.ConvertCoordinates(-10, 10);
            Point bottomLeft = coordinateHelper.ConvertCoordinates(-10, -10);
            Point topRight = coordinateHelper.ConvertCoordinates(10, 10);
            Point bottomRight = coordinateHelper.ConvertCoordinates(10, -10);

            // Assert
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.X, 60.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.Y, 50.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.X, 10.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.Y, 0.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.X, 10.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.Y, 100.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.X, 110.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.Y, 0.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.X, 110.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.Y, 100.0));
        }

        /// <summary>
        /// Simple tests on a portrait rectangle.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void RectanglePortraitSimple()
        {
            // Arrange
            double canvasWidth = 100.0;
            double canvasHeight = 120.0;
            double margin = 0.0;
            double fullInterval = 20.0;
            var coordinateHelper = new CoordinateHelper(canvasWidth, canvasHeight, margin, fullInterval);

            // Act
            Point origin = coordinateHelper.ConvertCoordinates(0.0, 0.0);
            Point topLeft = coordinateHelper.ConvertCoordinates(-10, 10);
            Point bottomLeft = coordinateHelper.ConvertCoordinates(-10, -10);
            Point topRight = coordinateHelper.ConvertCoordinates(10, 10);
            Point bottomRight = coordinateHelper.ConvertCoordinates(10, -10);

            // Assert
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.X, 50.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.Y, 60.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.X, 0.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.Y, 10.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.X, 0.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.Y, 110.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.X, 100.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.Y, 10.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.X, 100.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.Y, 110.0));
        }

        /// <summary>
        /// Simple tests on a landscape rectangle.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void RectangleLandscapeMargin()
        {
            // Arrange
            double canvasWidth = 120.0;
            double canvasHeight = 100.0;
            double margin = 5.0;
            double fullInterval = 20.0;
            var coordinateHelper = new CoordinateHelper(canvasWidth, canvasHeight, margin, fullInterval);

            // Act
            Point origin = coordinateHelper.ConvertCoordinates(0.0, 0.0);
            Point topLeft = coordinateHelper.ConvertCoordinates(-10, 10);
            Point bottomLeft = coordinateHelper.ConvertCoordinates(-10, -10);
            Point topRight = coordinateHelper.ConvertCoordinates(10, 10);
            Point bottomRight = coordinateHelper.ConvertCoordinates(10, -10);

            // Assert
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.X, 60.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.Y, 50.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.X, 15.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.Y, 5.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.X, 15.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.Y, 95.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.X, 105.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.Y, 5.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.X, 105.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.Y, 95.0));
        }

        /// <summary>
        /// Tests on a portrait rectangle with a margin.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void RectanglePortraitMargin()
        {
            // Arrange
            double canvasWidth = 100.0;
            double canvasHeight = 120.0;
            double margin = 5.0;
            double fullInterval = 20.0;
            var coordinateHelper = new CoordinateHelper(canvasWidth, canvasHeight, margin, fullInterval);

            // Act
            Point origin = coordinateHelper.ConvertCoordinates(0.0, 0.0);
            Point topLeft = coordinateHelper.ConvertCoordinates(-10, 10);
            Point bottomLeft = coordinateHelper.ConvertCoordinates(-10, -10);
            Point topRight = coordinateHelper.ConvertCoordinates(10, 10);
            Point bottomRight = coordinateHelper.ConvertCoordinates(10, -10);

            // Assert
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.X, 50.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(origin.Y, 60.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.X, 5.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topLeft.Y, 15.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.X, 5.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomLeft.Y, 105.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.X, 95.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(topRight.Y, 15.0));

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.X, 95.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(bottomRight.Y, 105.0));
        }
    }
}
