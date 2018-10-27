using System;
using System.Collections.Generic;
using Backend.BusinessLogic;
using Backend.BusinessLogic.Dots;
using Backend.BusinessLogic.Expressions;
using Backend.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence.Models;
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
            GameModel gameModel = new GameModel()
            {
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 0.0, Y = 0.0, },
                    new DotModel() { Kind = DotKind.Good, X = 4.0, Y = 0.0, },
                },
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
            };
            var maybeGame = Game.FromModel(gameModel);
            if (!maybeGame.HasValue)
            {
                throw new Exception("error in setup for" + nameof(this.TestSavingAndLoadingGame));
            }

            PollyController controller = new PollyController(maybeGame.Value);
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
            Assert.AreEqual(1, score1);
            Assert.AreEqual(2, score2);
            Assert.AreEqual(1, score3);
        }

        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void TestSavingAndLoadingGame()
        {
            // Arrange
            GameModel gameModel = new GameModel()
            {
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 0.0, Y = 0.0, },
                    new DotModel() { Kind = DotKind.Good, X = 1.0, Y = 1.0, },
                },
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
            };
            var maybeGame = Game.FromModel(gameModel);
            if (!maybeGame.HasValue)
            {
                throw new Exception("error in setup for" + nameof(this.TestSavingAndLoadingGame));
            }

            PollyController controller = new PollyController(maybeGame.Value, new InMemoryGameRepository());
            IExpression exprX = new BaseX();
            const string path = @"F:\temp\roundtripController.json";

            // Act
            controller.SetExpressionAtIndex(0, exprX.Print().Value);
            controller.UpdateData();
            controller.SaveGame(path);
            int score1 = controller.Score;

            controller.LoadGame(path);
            int score2 = controller.Score;

            // Assert
            Assert.AreEqual(3, score1);
            Assert.AreEqual(3, score2);
        }

        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void TestHandlingOfBadDots()
        {
            // Arrange
            GameModel gameModel = new GameModel()
            {
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 0.0, Y = 0.0, },
                    new DotModel() { Kind = DotKind.Bad, X = 4.0, Y = 0.0, },
                },
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
            };
            var maybeGame = Game.FromModel(gameModel);
            if (!maybeGame.HasValue)
            {
                throw new Exception("error in setup for" + nameof(this.TestSavingAndLoadingGame));
            }

            PollyController controller = new PollyController(maybeGame.Value);
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
            Assert.AreEqual(1, score1);
            Assert.AreEqual(-1, score2);
            Assert.AreEqual(1, score3);
        }
    }
}
