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

            // Assert
            Assert.IsTrue(game != null);
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
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
                DotModels = new List<DotModel>()
                {
                    new DotModel()
                    {
                        Kind = DotKind.Bad,
                        X = 0.1,
                        Y = -0.2,
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
        /// Test that a roundtrip starting from a valid model works.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldRoundTripFromValidModel()
        {
            // Arrange
            const string expression0 = "exp(x)";
            const string expression2 = "x*x";
            var gameModel = new GameModel()
            {
                ExpressionStrings = new List<string>() { expression0, string.Empty, expression2, string.Empty, string.Empty, },
                DotModels = new List<DotModel>()
                {
                    new DotModel()
                    {
                        Kind = DotKind.Bad,
                        X = 0.1,
                        Y = -0.2,
                    },
                },
            };

            // Act
            var game = Game.FromModel(gameModel);
            var resultModel = game.HasValue ? game.Value.GetModel() : null;

            // Assert
            Assert.IsTrue(game.HasValue);
            Assert.IsNotNull(resultModel);
            Assert.IsTrue(gameModel.Equals(resultModel));
        }
    }
}
