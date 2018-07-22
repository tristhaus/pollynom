using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Expressions;
using PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions;
using PollyNomTest.Helper;
using System;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to the results
    /// of evaluating <see cref="IExpression"/> instances.
    /// </summary>
    [TestClass]
    public class EvaluatingTest
    {
        [TestMethod]
        public void Evaluate01()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression01();
            double expectedValue = 0.2;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue? result.Value: 0.0;

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate02()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression02();
            double expectedValue = -0.00205809;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue? result.Value: 0.0;

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate03()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression03();
            double expectedValue = 2.2;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue? result.Value: 0.0;

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate04()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression04();
            double expectedValue = 1.21;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue? result.Value: 0.0;

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate05()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression05();
            double expectedValue = 1.003182058;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue? result.Value: 0.0;

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(expectedValue, number));
        }


        [TestMethod]
        public void Evaluate06()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression06();
            double expectedValue = -1.9;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue? result.Value: 0.0;

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate07()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression07();
            double expectedValue = 2.1;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue? result.Value: 0.0;

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate08()
        {
            // Arrange
            IExpression expr = TestExpressionBuilder.Expression08();
            double expectedValue = 4.1;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue? result.Value: 0.0;

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void EvaluateExponentialAndLogarithm()
        {
            // Arrange
            IExpression exp = new Exponential(new BaseX());
            IExpression log = new NaturalLogarithm(new BaseX());
            IExpression expLog = new Exponential(new NaturalLogarithm(new BaseX()));
            IExpression logExp = new NaturalLogarithm(new Exponential(new BaseX()));

            // Act
            var resultE = exp.Evaluate(1.0);
            var resultZero = log.Evaluate(1.0);
            var resultTwo1 = expLog.Evaluate(2.0);
            var resultHundred1 = expLog.Evaluate(100.0);
            var resultTwo2 = logExp.Evaluate(2.0);
            var resultHundred2 = logExp.Evaluate(100.0);

            var noResult = log.Evaluate(-1.0);

            // Assert
            Assert.IsTrue(resultE.HasValue);
            Assert.IsTrue(resultZero.HasValue);
            Assert.IsTrue(resultTwo1.HasValue);
            Assert.IsTrue(resultHundred1.HasValue);
            Assert.IsTrue(resultTwo2.HasValue);
            Assert.IsTrue(resultHundred2.HasValue);
            Assert.IsFalse(noResult.HasValue);

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultE.Value, System.Math.E));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultZero.Value, 0.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultTwo1.Value, 2.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultHundred1.Value, 100.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultTwo2.Value, 2.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultHundred2.Value, 100.0));
        }

        [TestMethod]
        public void EvaluateTrigonometricFunctions()
        {
            // Arrange
            IExpression sin = new Sine(new BaseX());
            IExpression cos = new Cosine(new BaseX());
            IExpression tan = new Tangent(new BaseX());

            // Act
            var resultSin1 = sin.Evaluate((1.0 / 6.0) * Math.PI);
            var resultSin2 = sin.Evaluate((1.0 / 6.0) * Math.PI + 2 * Math.PI);
            var resultCos1 = cos.Evaluate((1.0 / 3.0) * Math.PI);
            var resultCos2 = cos.Evaluate((1.0 / 3.0) * Math.PI + 2 * Math.PI);
            var resultTan1 = tan.Evaluate((1.0 / 4.0) * Math.PI);
            var resultTan2 = tan.Evaluate((1.0 / 4.0) * Math.PI + 2 * Math.PI);

            // Assert
            Assert.IsTrue(resultSin1.HasValue);
            Assert.IsTrue(resultSin2.HasValue);
            Assert.IsTrue(resultCos1.HasValue);
            Assert.IsTrue(resultCos2.HasValue);
            Assert.IsTrue(resultTan1.HasValue);
            Assert.IsTrue(resultTan2.HasValue);

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultSin1.Value, 0.5));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultSin2.Value, 0.5));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultCos1.Value, 0.5));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultCos2.Value, 0.5));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultTan1.Value, 1.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultTan2.Value, 1.0)); 
        }

        [TestMethod]
        public void EvaluateAbsoluteValue()
        {
            // Arrange
            IExpression abs = new AbsoluteValue(new BaseX());

            // Act
            var resultPositive = abs.Evaluate(1.0);
            var resultZero = abs.Evaluate(0.0);
            var resultNegative = abs.Evaluate(-1.0);

            // Assert
            Assert.IsTrue(resultPositive.HasValue);
            Assert.IsTrue(resultZero.HasValue);
            Assert.IsTrue(resultNegative.HasValue);

            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultPositive.Value, 1.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultZero.Value, 0.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(resultNegative.Value, 1.0));
        }
    }
}
