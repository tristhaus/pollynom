using System;
using System.Collections.Generic;
using Backend.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence.Models;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to the Game domain model.
    /// </summary>
    [TestClass]
    public class GameTest
    {
        /// <summary>
        /// Test that the NewRandom() method provides a valid Game.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveValidNewGame()
        {
            // Arrange
            // Act
            var game = Game.NewRandom();
            var gameModel = game.GetModel();

            // Assert
            Assert.IsTrue(game != null);
            Assert.IsTrue(gameModel != null);
        }

        /// <summary>
        /// Test that the loading method provides a valid game if given valid input.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveValidGameFromValidModel()
        {
            // Arrange
            var gameModel = new GameModel()
            {
                Id = Guid.Parse("fd655e2e-3be5-4453-8542-9e57fdbe9548"),
                Signature = "8d0cc70f90bc27fabc2c0e4911307f0a13450a0070c1447fac3b1449b95c013f",
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
                DotModels = new List<DotModel>()
                {
                    new DotModel()
                    {
                        Kind = DotKind.Bad,
                        X = 3.5,
                        Y = 0.5,
                    },
                },
            };

            // Act
            var game = Game.FromModel(gameModel);

            // Assert
            Assert.IsTrue(game.HasValue);
        }

        /// <summary>
        /// Test that the loading method provides an invalid game if the number of expressionStrings is wrong.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveInvalidGameFromModelWithWrongNumberOfExpressionStrings()
        {
            // Arrange
            var gameModel = new GameModel()
            {
                ExpressionStrings = new List<string>(0),
                DotModels = new List<DotModel>()
                {
                    new DotModel()
                    {
                        X = 0.1,
                        Y = -0.2,
                    },
                },
            };

            // Act
            var game = Game.FromModel(gameModel);

            // Assert
            Assert.IsFalse(game.HasValue);
        }

        /// <summary>
        /// Test that a roundtrip starting from a new random game.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldRoundTripFromNewGame()
        {
            // Arrange
            const string expression0 = "exp(x)";
            const string expression2 = "x*x";

            Game game = Game.NewRandom(1, 1);
            game.ExpressionStrings[0] = expression0;
            game.ExpressionStrings[2] = expression2;

            // Act
            var model1 = game.GetModel();
            var resultGame = Game.FromModel(model1);
            var model2 = game.GetModel();

            // Assert
            Assert.IsTrue(resultGame.HasValue);
            Assert.AreEqual(expression0, resultGame.Value.ExpressionStrings[0]);
            Assert.AreEqual(expression2, resultGame.Value.ExpressionStrings[2]);
            Assert.IsTrue(model1.Equals(model2));
        }
    }
}
