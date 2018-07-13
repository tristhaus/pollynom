using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Dots;
using PollyNom.BusinessLogic.Expressions;
using PollyNom.Controller;

namespace PollyNomTest
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void TestSequentialAdditionAndSubtraction()
        {
            // Arrange
            List<IDot> testDots = new List<IDot> { new GoodDot(0.0, 0.0), new GoodDot(4.0, 0.0)};
            PollyController controller = new PollyController(testDots);
            IExpression exprX = new BaseX();
            IExpression exprConst = new Constant(0.0);

            // Act
            controller.UpdateExpression(exprX.Print().Value());
            int score1 = controller.Score;
            controller.UpdateExpression(exprConst.Print().Value());
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
