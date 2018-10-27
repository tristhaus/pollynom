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
                Id = Guid.Parse("228b4ee7-6975-4203-8cfa-cd36a5413537"),
                Signature = "7a6f40d2e98c213a46ea6ce6e5c5fbbb498cdd5a5359378a04d5e9b896e09cb1",
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 3.5, Y = 9.5, },
                    new DotModel() { Kind = DotKind.Good, X = -2.5, Y = 1.5, },
                    new DotModel() { Kind = DotKind.Bad, X = 0.5, Y = -2.5, },
                },
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
            };
            var maybeGame = Game.FromModel(gameModel);
            if (!maybeGame.HasValue)
            {
                throw new Exception("error in setup for" + nameof(this.TestSavingAndLoadingGame));
            }

            PollyController controller = new PollyController(maybeGame.Value);
            IExpression exprConst1 = new Constant(9.5);
            IExpression exprConst2 = new Constant(1.5);

            // Act
            controller.SetExpressionAtIndex(0, exprConst1.Print().Value);
            controller.UpdateData();
            int score1 = controller.Score;

            controller.SetExpressionAtIndex(1, exprConst2.Print().Value);
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

        /// <summary>
        /// Test the saving and loading of a game via the controller.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void TestSavingAndLoadingGame()
        {
            // Arrange
            Game game = Game.NewRandom(2, 0);
            GameModel gameModel = game.GetModel();

            PollyController controller = new PollyController(game, new InMemoryGameRepository());
            const string path = @"F:\temp\roundtripController.json";

            // Act
            controller.SetExpressionAtIndex(0, "10*sin(30*x)");
            controller.UpdateData();
            controller.SaveGame(path);
            int score1 = controller.Score;

            controller.LoadGame(path);
            int score2 = controller.Score;

            // Assert
            Assert.AreEqual(3, score1);
            Assert.AreEqual(3, score2);
        }

        /// <summary>
        /// Test that a controller handles bad dots correctly.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void TestHandlingOfBadDots()
        {
            // Arrange
            GameModel gameModel = new GameModel()
            {
                Id = Guid.Parse("05426060-ef1f-4080-81d3-03471a3e802d"),
                Signature = "f6c53775100aee101a8d30852f11e6ea950a5beb09cbb815761deee3a23399b8",
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = -1.5, Y = 8.5, },
                    new DotModel() { Kind = DotKind.Bad, X = 3.5, Y = 7.5, },
                },
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
            };
            var maybeGame = Game.FromModel(gameModel);
            if (!maybeGame.HasValue)
            {
                throw new Exception("error in setup for" + nameof(this.TestSavingAndLoadingGame));
            }

            PollyController controller = new PollyController(maybeGame.Value);
            IExpression exprConstGood = new Constant(8.5);
            IExpression exprConstBad = new Constant(7.5);

            // Act
            controller.SetExpressionAtIndex(0, exprConstGood.Print().Value);
            controller.UpdateData();
            int score1 = controller.Score;

            controller.SetExpressionAtIndex(1, exprConstBad.Print().Value);
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
