using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic;
using PollyNomTest.Helper;

namespace PollyNomTest
{
    [TestClass]
    public class Evaluating
    {
        [TestMethod]
        public void Evaluate01()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression01();
            double expectedValue = 0.2;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue() ? result.Value() : 0.0;

            // Assert
            Assert.IsTrue(result.HasValue());
            Assert.IsTrue(DoubleComparer.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate02()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression02();
            double expectedValue = -0.00205809;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue() ? result.Value() : 0.0;

            // Assert
            Assert.IsTrue(result.HasValue());
            Assert.IsTrue(DoubleComparer.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate03()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression03();
            double expectedValue = 2.2;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue() ? result.Value() : 0.0;

            // Assert
            Assert.IsTrue(result.HasValue());
            Assert.IsTrue(DoubleComparer.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate04()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression04();
            double expectedValue = 1.21;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue() ? result.Value() : 0.0;

            // Assert
            Assert.IsTrue(result.HasValue());
            Assert.IsTrue(DoubleComparer.IsApproximatelyEqual(expectedValue, number));
        }

        [TestMethod]
        public void Evaluate05()
        {
            // Arrange
            Expression expr = TestExpressionBuilder.Expression05();
            double expectedValue = 1.003182058;

            // Act
            var result = expr.Evaluate(0.1);
            double number = result.HasValue() ? result.Value() : 0.0;

            // Assert
            Assert.IsTrue(result.HasValue());
            Assert.IsTrue(DoubleComparer.IsApproximatelyEqual(expectedValue, number));
        }
    }
}
