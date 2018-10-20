using System.Collections.Generic;
using Backend.BusinessLogic;
using Backend.BusinessLogic.Dots;
using Backend.BusinessLogic.Expressions;
using Backend.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersistenceTest;

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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void TestSequentialAdditionAndRemoval()
        {
            // Arrange
            List<IDot> testDots = new List<IDot> { new GoodDot(0.0, 0.0), new GoodDot(4.0, 0.0) };
            PollyController controller = new PollyController(testDots);
            IExpression exprX = new BaseX();
            IExpression exprConst = new Constant(0.0);

            // Act
            controller.SetExpressionAtIndex(0, exprX.Print().Value);
            controller.UpdateData();
            int score1 = controller.Score;

            controller.SetExpressionAtIndex(1, exprConst.Print().Value);
            controller.UpdateData();
            int score2 = controller.Score;

            controller.SetExpressionAtIndex(1, string.Empty);
            controller.UpdateData();
            int score3 = controller.Score;

            // Assert
            Assert.AreEqual(score1, 1);
            Assert.AreEqual(score2, 2);
            Assert.AreEqual(score3, 1);
        }

        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void TestSavingAndLoadingGame()
        {
            // Arrange
            List<IDot> testDots = new List<IDot> { new GoodDot(0.0, 0.0), new GoodDot(1.0, 1.0) };
            PollyController controller = new PollyController(testDots, new InMemoryGameRepository());
            IExpression exprX = new BaseX();
            const string path = @"F:\temp\roundtripController.json";

            // Act
            controller.SetExpressionAtIndex(0, exprX.Print().Value);
            controller.UpdateData();
            controller.SaveGame(path);
            int score1 = controller.Score;

            controller.LoadGame(path);
            int score2 = controller.Score;

            controller.SetExpressionAtIndex(0, exprX.Print().Value);
            controller.UpdateData();
            int score3 = controller.Score;

            // Assert
            Assert.AreEqual(score1, 3);
            Assert.AreEqual(score2, 0);
            Assert.AreEqual(score3, 3);
        }
    }
}
