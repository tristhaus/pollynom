﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic;
using PollyNomTest.Helper;

namespace PollyNomTest
{
    [TestClass]
    public class Outputting
    {
        [TestMethod]
        public void Print01()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression01();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"{2.0}*x", result);
        }

        [TestMethod]
        public void Print02()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression02();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"{2}*x^{3}/(x-{2}^x)", result);
        }

        [TestMethod]
        public void Print03()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression03();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"{2}*(x+{1})", result);
        }

        [TestMethod]
        public void Print04()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression04();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"(x+{1})^{2}", result);
        }

        [TestMethod]
        public void Print05()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression05();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"(x+{1})^(x/{3})", result);
        }
    }
}
