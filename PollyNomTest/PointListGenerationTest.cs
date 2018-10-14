using Backend.BusinessLogic;
using Backend.BusinessLogic.Expressions;
using Backend.BusinessLogic.Expressions.SingleArgumentFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestInfrastructure;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to the generation of lists of point lists.
    /// </summary>
    [TestClass]
    public class PointListGenerationTest
    {
        /// <summary>
        /// Tests the evaluation of x^2 into a single list.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void XSquared_HasOneList()
        {
            // Arrange
            IExpression xSquared = new Power(new BaseX(), new Constant(2.0));
            PointListGenerator pointList = new PointListGenerator(xSquared, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalPoints();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result[0].Count >= 1);
        }

        /// <summary>
        /// Tests the evaluation of 1/x into two lists.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void OneOverX_HasTwoLists()
        {
            // Arrange
            IExpression oneOverX = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));
            PointListGenerator pointList = new PointListGenerator(oneOverX, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalPoints();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result[0].Count >= 1);
            Assert.IsTrue(result[1].Count >= 1);
        }

        /// <summary>
        /// Tests the evaluation of x^(0.5) into a single list and a decent starting value.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void RootX_HasOneList()
        {
            // Arrange
            IExpression rootX = new Power(new BaseX(), new Constant(0.5));
            PointListGenerator pointList = new PointListGenerator(rootX, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalPoints();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result[0].Count >= 1);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(result[0].Points[0].X, 0.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(result[0].Points[0].Y, 0.0));
        }

        /// <summary>
        /// Tests the evaluation of ln(x) into a single list with a decent starting value.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void LnX_HasOneListAndStartsSufficientlyLow()
        {
            // Arrange
            IExpression lnX = new NaturalLogarithm(new BaseX());
            PointListGenerator pointList = new PointListGenerator(lnX, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalPoints();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result[0].Count >= 1);
            Assert.IsTrue(result[0].Points[0].Y < -10);
        }
    }
}
