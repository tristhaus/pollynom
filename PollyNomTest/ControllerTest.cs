using System.Collections.Generic;
using Backend.BusinessLogic;
using Backend.BusinessLogic.Dots;
using Backend.BusinessLogic.Expressions;
using Backend.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to controller methods.
    /// </summary>
    [TestClass]
    public class ControllerTest
    {
        /// <summary>
        /// Tests the sequential addition and removal of expressions.
        /// </summary>
        [TestMethod]
        public void TestSequentialAdditionAndRemoval()
        {
            // Arrange
            List<IDot> testDots = new List<IDot> { new GoodDot(0.0, 0.0), new GoodDot(4.0, 0.0) };
            PollyController controller = new PollyController(testDots);
            IExpression exprX = new BaseX();
            IExpression exprConst = new Constant(0.0);

            // Act
            controller.UpdateExpression(exprX.Print().Value);
            int score1 = controller.Score;
            controller.UpdateExpression(exprConst.Print().Value);
            int score2 = controller.Score;
            controller.UpdateExpression(string.Empty);
            int score3 = controller.Score;

            // Assert
            Assert.AreEqual(score1, 1);
            Assert.AreEqual(score2, 2);
            Assert.AreEqual(score3, 1);
        }
    }
}
