using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;

using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Expressions;
using PollyNom.View;

namespace PollyNomTest
{
    [TestClass]
    public class PointListGenerationTest
    {
        [TestMethod]
        public void Constant_HasOneListAndTheCorrectValue()
        {
            // Arrange
            IExpression constant = new Constant(1.3);
            PointListGenerator pointList = new PointListGenerator(constant, -1.0, 1.0, 1000.0);

            // Act
            List<List<PointF>> result = PollyFormHelper.ConvertToScaledPoints(pointList.ObtainListsOfLogicalsPoints());

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
            Assert.IsTrue(Helper.DoubleEquality.IsApproximatelyEqual(1.3, result[0][0].Y));
        }

        [TestMethod]
        public void Constant_HasOneListAndTheCorrectScaledValue()
        {
            // Arrange
            IExpression constant = new Constant(1.3);
            PointListGenerator pointList = new PointListGenerator(constant, -1.0, 1.0, 1000.0);

            // Act
            List<List<PointF>> result = PollyFormHelper.ConvertToScaledPoints(pointList.ObtainListsOfLogicalsPoints(), 0.5f, 2.0f);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
            Assert.IsTrue(Helper.DoubleEquality.IsApproximatelyEqual(2.6, result[0][0].Y));
        }

        [TestMethod]
        public void XSquared_HasOneList()
        {
            // Arrange
            IExpression xSquared = new Power(new BaseX(), new Constant(2.0));
            PointListGenerator pointList = new PointListGenerator(xSquared, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalsPoints();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
        }

        [TestMethod]
        public void OneOverX_HasTwoLists()
        {
            // Arrange
            IExpression oneOverX = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));
            PointListGenerator pointList = new PointListGenerator(oneOverX, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalsPoints();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
            Assert.IsTrue(1 <= result[1].Count);
        }


        [TestMethod]
        public void RootX_HasOneList()
        {
            // Arrange
            IExpression RootX = new Power(new BaseX(), new Constant(0.5));
            PointListGenerator pointList = new PointListGenerator(RootX, -1.0, 1.0, 1000.0);

            // Act
            var result = pointList.ObtainListsOfLogicalsPoints();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(1 <= result[0].Count);
        }
    }
}
