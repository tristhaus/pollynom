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

﻿using Backend.BusinessLogic;
using Backend.BusinessLogic.Expressions;
using Backend.BusinessLogic.Expressions.SingleArgumentFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNomTest.Helper;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to textual output
    /// of <see cref="IExpression"/> instances.
    /// </summary>
    [TestClass]
    public class PrintingTest
    {
        /// <summary>
        /// Tests the printing of <see cref="TestExpressionBuilder.Expression01"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Print01()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression01();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"2*x", result);
        }

        /// <summary>
        /// Tests the printing of <see cref="TestExpressionBuilder.Expression02"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Print02()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression02();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"2*x^3/(x-2^x)", result);
        }

        /// <summary>
        /// Tests the printing of <see cref="TestExpressionBuilder.Expression03"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Print03()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression03();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"2*(x+1)", result);
        }

        /// <summary>
        /// Tests the printing of <see cref="TestExpressionBuilder.Expression04"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Print04()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression04();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"(x+1)^2", result);
        }

        /// <summary>
        /// Tests the printing of <see cref="TestExpressionBuilder.Expression05"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Print05()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression05();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"(x+1)^(x/3)", result);
        }

        /// <summary>
        /// Tests the printing of <see cref="TestExpressionBuilder.Expression06"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Print06()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression06();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"x-1+2-3", result);
        }

        /// <summary>
        /// Tests the printing of <see cref="TestExpressionBuilder.Expression07"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Print07()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression07();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"x+1-2+3", result);
        }

        /// <summary>
        /// Tests the printing of <see cref="TestExpressionBuilder.Expression08"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void Print08()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression08();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"x+1-4+7", result);
        }

        /// <summary>
        /// Tests the printing of an exponential function and natural logarithm.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void PrintExponentialAndLogarithm()
        {
            // Arrange
            IExpression exp = new Exponential(new BaseX());
            IExpression log = new NaturalLogarithm(new BaseX());
            IExpression expLog = new Exponential(new NaturalLogarithm(new BaseX()));

            // Act
            string resultExp = ExpressionPrinter.PrintExpression(exp);
            string resultLog = ExpressionPrinter.PrintExpression(log);
            string resultExpLog = ExpressionPrinter.PrintExpression(expLog);

            // Assert
            Assert.AreEqual<string>("exp(x)", resultExp);
            Assert.AreEqual<string>("ln(x)", resultLog);
            Assert.AreEqual<string>("exp(ln(x))", resultExpLog);
        }

        /// <summary>
        /// Tests the printing of the sine function.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void PrintSine()
        {
            // Arrange
            IExpression sin = new Sine(new BaseX());

            // Act
            string resultExp = ExpressionPrinter.PrintExpression(sin);

            // Assert
            Assert.AreEqual<string>("sin(x)", resultExp);
        }

        /// <summary>
        /// Tests the printing of the cosine function.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void PrintCosine()
        {
            // Arrange
            IExpression cos = new Cosine(new BaseX());

            // Act
            string resultExp = ExpressionPrinter.PrintExpression(cos);

            // Assert
            Assert.AreEqual<string>("cos(x)", resultExp);
        }

        /// <summary>
        /// Tests the printing of the tangent function.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void PrintTangent()
        {
            // Arrange
            IExpression tan = new Tangent(new BaseX());

            // Act
            string resultExp = ExpressionPrinter.PrintExpression(tan);

            // Assert
            Assert.AreEqual<string>("tan(x)", resultExp);
        }

        /// <summary>
        /// Tests the printing of the absolute value function.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void PrintAbsoluteValue()
        {
            // Arrange
            IExpression abs = new AbsoluteValue(new BaseX());

            // Act
            string resultExp = ExpressionPrinter.PrintExpression(abs);

            // Assert
            Assert.AreEqual<string>("abs(x)", resultExp);
        }
    }
}
