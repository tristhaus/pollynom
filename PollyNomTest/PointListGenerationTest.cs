using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;

using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Expressions;
using PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions;
using PollyNom.View;
using PollyNomTest.Helper;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to the generation of lists of point lists.
    /// </summary>
    [TestClass]
    public class PointListGenerationTest
    {
        /// <summary>
        /// Tests the correct evaluation of a constant to unscaled points.
        /// </summary>
        [TestMethod]
        public void Constant_HasOneListAndTheCorrectValue()
        {
            // Arrange
            IExpression constant = new Constant(1.3);
            PointListGenerator pointList = new PointListGenerator(constant, -1.0, 1.0, 1000.0);

            // Act
            List<List<PointF>> result = PollyFormHelper.ConvertToScaledPoints(pointList.ObtainListsOfLogicalPoints());

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
            Assert.IsTrue(Helper.DoubleEquality.IsApproximatelyEqual(1.3, result[0][0].Y));
        }

        /// <summary>
        /// Tests the correct evaluation of a constant to scaled points.
        /// </summary>
        [TestMethod]
        public void Constant_HasOneListAndTheCorrectScaledValue()
        {
            // Arrange
            IExpression constant = new Constant(1.3);
            PointListGenerator pointList = new PointListGenerator(constant, -1.0, 1.0, 1000.0);

            // Act
            List<List<PointF>> result = PollyFormHelper.ConvertToScaledPoints(pointList.ObtainListsOfLogicalPoints(), 0.5f, 2.0f);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
            Assert.IsTrue(Helper.DoubleEquality.IsApproximatelyEqual(2.6, result[0][0].Y));
        }

        /// <summary>
        /// Tests the evaluation of x^2 into a single list.
        /// </summary>
        [TestMethod]
        public void XSquared_HasOneList()
        {
            // Arrange
            IExpression xSquared = new Power(new BaseX(), new Constant(2.0));
            PointListGenerator pointList = new PointListGenerator(xSquared, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalPoints();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
        }

        /// <summary>
        /// Tests the evaluation of 1/x into two lists.
        /// </summary>
        [TestMethod]
        public void OneOverX_HasTwoLists()
        {
            // Arrange
            IExpression oneOverX = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));
            PointListGenerator pointList = new PointListGenerator(oneOverX, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalPoints();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
            Assert.IsTrue(1 <= result[1].Count);
        }

        /// <summary>
        /// Tests the evaluation of x^(0.5) into a single list and a decent starting value.
        /// </summary>
        [TestMethod]
        public void RootX_HasOneList()
        {
            // Arrange
            IExpression RootX = new Power(new BaseX(), new Constant(0.5));
            PointListGenerator pointList = new PointListGenerator(RootX, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalPoints();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(result[0].Points[0].X, 0.0));
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(result[0].Points[0].Y, 0.0));
        }

        /// <summary>
        /// Tests the evaluation of ln(x) into a single list with a decent starting value.
        /// </summary>
        [TestMethod]
        public void LnX_HasOneListAndStartsSufficientlyLow()
        {
            // Arrange
            IExpression LnX = new NaturalLogarithm(new BaseX());
            PointListGenerator pointList = new PointListGenerator(LnX, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalPoints();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
            Assert.IsTrue(result[0].Points[0].Y < -10);
        }

    }
}
