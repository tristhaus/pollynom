﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Expressions;
using PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions;
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
        [TestMethod]
        public void Print01()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression01();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"{2.0}*x", result);
        }

        [TestMethod]
        public void Print02()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression02();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"{2}*x^{3}/(x-{2}^x)", result);
        }

        [TestMethod]
        public void Print03()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression03();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"{2}*(x+{1})", result);
        }

        [TestMethod]
        public void Print04()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression04();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"(x+{1})^{2}", result);
        }

        [TestMethod]
        public void Print05()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression05();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"(x+{1})^(x/{3})", result);
        }

        [TestMethod]
        public void Print06()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression06();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"x-{1}+{2}-{3}", result);
        }

        [TestMethod]
        public void Print07()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression07();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"x+{1}-{2}+{3}", result);
        }

        [TestMethod]
        public void Print08()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression08();

            // Act
            string result = ExpressionPrinter.PrintExpression(expr);

            // Assert
            Assert.AreEqual<string>($"x+{1}-{4}+{7}", result);
        }

        [TestMethod]
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

        [TestMethod]
        public void PrintSine()
        {
            // Arrange
            IExpression sin = new Sine(new BaseX());

            // Act
            string resultExp = ExpressionPrinter.PrintExpression(sin);

            // Assert
            Assert.AreEqual<string>("sin(x)", resultExp);
        }

        [TestMethod]
        public void PrintCosine()
        {
            // Arrange
            IExpression cos = new Cosine(new BaseX());

            // Act
            string resultExp = ExpressionPrinter.PrintExpression(cos);

            // Assert
            Assert.AreEqual<string>("cos(x)", resultExp);
        }

        [TestMethod]
        public void PrintTangent()
        {
            // Arrange
            IExpression tan = new Tangent(new BaseX());

            // Act
            string resultExp = ExpressionPrinter.PrintExpression(tan);

            // Assert
            Assert.AreEqual<string>("tan(x)", resultExp);
        }

        [TestMethod]
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
