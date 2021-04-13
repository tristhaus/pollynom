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

﻿using System.Collections.Generic;
using Backend.BusinessLogic;
using Backend.BusinessLogic.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence.Models;
using TestInfrastructure;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to whether an expression hits a dot.
    /// </summary>
    [TestClass]
    public class DotHittingTest
    {
        private const double StartX = -1.0;
        private const double EndX = 1.0;
        private const double Limits = 1000.0;

        /// <summary>
        /// Tests hitting of a dot at tangential points
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void TrivialBorderCases()
        {
            // Arrange
            IDot originDot = new Dot(DotKind.Good, 0.0, 0.0);

            IExpression top = new Constant(0.25);
            IExpression bottom = new Constant(-0.25);

            // +1000 * x - 250
            IExpression right = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(+1000.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(250.0)));

            // -1000 * x - 250
            IExpression left = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-1000.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(250.0)));

            IExpression topMiss = new Constant(0.3);
            IExpression bottomMiss = new Constant(-0.3);

            // +1000 * x - 500
            IExpression rightMiss = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(+1000.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(500.0)));

            // -1000 * x - 500
            IExpression leftMiss = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-1000.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(500.0)));

            // Act
            bool topResult = originDot.IsHit(top, null);
            bool bottomResult = originDot.IsHit(bottom, null);
            bool rightResult = originDot.IsHit(right, null);
            bool leftResult = originDot.IsHit(left, null);
            bool topMissResult = originDot.IsHit(topMiss, null);
            bool bottomMissResult = originDot.IsHit(bottomMiss, null);
            bool rightMissResult = originDot.IsHit(rightMiss, null);
            bool leftMissResult = originDot.IsHit(leftMiss, null);

            // Assert
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(originDot.Radius, 0.25));

            Assert.IsTrue(topResult);
            Assert.IsTrue(bottomResult);
            Assert.IsTrue(rightResult);
            Assert.IsTrue(leftResult);
            Assert.IsFalse(topMissResult);
            Assert.IsFalse(bottomMissResult);
            Assert.IsFalse(rightMissResult);
            Assert.IsFalse(leftMissResult);
        }

        /// <summary>
        /// Non-trivial case of a singularity
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Singularity1()
        {
            // Arrange
            IDot originDot = new Dot(DotKind.Good, 0.0, 0.0);

            IExpression plainOneOverX = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));

            PointListGenerator pointListGenerator = new PointListGenerator(plainOneOverX, DotHittingTest.StartX, DotHittingTest.EndX, DotHittingTest.Limits);

            List<ListPointLogical> logicalPointLists = pointListGenerator.ObtainListsOfLogicalPoints();

            // Act
            bool plainOneOverXResult = originDot.IsHit(plainOneOverX, logicalPointLists);

            // Assert
            Assert.IsFalse(plainOneOverXResult);
        }

        /// <summary>
        /// Non-trivial case of a singularity
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Singularity2()
        {
            // Arrange
            IDot originDot = new Dot(DotKind.Good, 0.0, 0.0);

            IExpression oneOverXMinus10 = new Add(
                new Add.AddExpression(
                    Add.AddExpression.Signs.Plus,
                    new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()))),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(10.0)));

            PointListGenerator pointListGenerator = new PointListGenerator(oneOverXMinus10, DotHittingTest.StartX, DotHittingTest.EndX, DotHittingTest.Limits);

            List<ListPointLogical> logicalPointLists = pointListGenerator.ObtainListsOfLogicalPoints();

            // Act
            bool oneOverXMinus10Result = originDot.IsHit(oneOverXMinus10, logicalPointLists);

            // Assert
            Assert.IsTrue(oneOverXMinus10Result);
        }
    }
}
