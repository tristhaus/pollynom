using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic;
using PollyNomTest.Helper;

namespace PollyNomTest
{
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
    }
}
