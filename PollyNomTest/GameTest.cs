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
        /// Test that the loading method provides a valid game and that the expression is disregarded for confirming validity.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveValidGameFromValidModelRegardlessOfExpressionsContained()
        {
            // Arrange
            var gameModel1 = new GameModel()
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

            var gameModel2 = new GameModel()
            {
                Id = Guid.Parse("fd655e2e-3be5-4453-8542-9e57fdbe9548"),
                Signature = "8d0cc70f90bc27fabc2c0e4911307f0a13450a0070c1447fac3b1449b95c013f",
                ExpressionStrings = new List<string>() { "a", string.Empty, "b", string.Empty, "c" },
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
            var game1 = Game.FromModel(gameModel1);
            var game2 = Game.FromModel(gameModel2);

            // Assert
            Assert.IsTrue(game1.HasValue);
            Assert.IsTrue(game2.HasValue);
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
                Id = Guid.Parse("ef001350-37b0-403e-8fed-fa2d32e43b88"),
                Signature = "7758254F2E385AD9315CE8A5D7E333FF7D2F9DF47E105188A525B94EC0B4733B",
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, },
                DotModels = new List<DotModel>()
                {
                    new DotModel()
                    {
                        X = 1.5,
                        Y = -2.5,
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

        /// <summary>
        /// Test that the loading method provides an invalid game if the signature is bad.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveInvalidGameFromModelWithBadSignature()
        {
            // Arrange
            var gameModel = new GameModel()
            {
                Id = Guid.Parse("174de19e-9914-48a1-83ba-c5f845f2124c"),
                Signature = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
                DotModels = new List<DotModel>()
                {
                    new DotModel()
                    {
                        Kind = DotKind.Good,
                        X = 4.5,
                        Y = -2.5,
                    },
                },
            };

            // Act
            var game = Game.FromModel(gameModel);

            // Assert
            Assert.IsFalse(game.HasValue);
        }

        /// <summary>
        /// Test that the loading method provides an invalid game if the Id was altered after signing.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveInvalidGameFromModelWithDifferingId()
        {
            // Arrange
            var gameModel = new GameModel()
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Signature = "A8F1FBE22AFE0D3D0882B0AD8298A3DD8B2DBFB4A3DF42D32FE716032B7B3339",
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
                DotModels = new List<DotModel>()
                {
                    new DotModel()
                    {
                        Kind = DotKind.Good,
                        X = 4.5,
                        Y = -2.5,
                    },
                },
            };

            // Act
            var game = Game.FromModel(gameModel);

            // Assert
            Assert.IsFalse(game.HasValue);
        }

        /// <summary>
        /// Test that the loading method provides a valid game from a realistic model.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveValidGameFromRealisticModel()
        {
            // Arrange
            var gameModel = new GameModel()
            {
                Id = Guid.Parse("abc2411f-2ef4-4196-a51d-3c215c2494af"),
                Signature = "699D58A4E7C5929A693B4A9C6DF0BA151A19848F4E6A41DCB4722BB7F3BF384A",
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = -1.5, Y = -1.5, },
                    new DotModel() { Kind = DotKind.Good, X = -1.5, Y = +3.5, },
                    new DotModel() { Kind = DotKind.Good, X = -8.5, Y = -9.5, },
                    new DotModel() { Kind = DotKind.Good, X = -5.5, Y = -0.5, },
                    new DotModel() { Kind = DotKind.Good, X = -8.5, Y = -0.5, },
                    new DotModel() { Kind = DotKind.Good, X = +7.5, Y = +6.5, },
                    new DotModel() { Kind = DotKind.Good, X = +8.5, Y = +1.5, },
                    new DotModel() { Kind = DotKind.Good, X = -7.5, Y = -4.5, },
                    new DotModel() { Kind = DotKind.Bad,  X = +5.5, Y = +7.5, },
                    new DotModel() { Kind = DotKind.Bad,  X = +2.5, Y = +0.5, },
                },
            };

            // Act
            var game = Game.FromModel(gameModel);

            // Assert
            Assert.IsTrue(game.HasValue);
        }

        /// <summary>
        /// Test that the loading method provides an invalid game if the dots were altered after signing.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveInvalidGameFromModelWithAlteredDot1()
        {
            // Arrange
            var gameModel = new GameModel()
            {
                Id = Guid.Parse("abc2411f-2ef4-4196-a51d-3c215c2494af"),
                Signature = "699D58A4E7C5929A693B4A9C6DF0BA151A19848F4E6A41DCB4722BB7F3BF384A",
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = -1.5, Y = -1.5, },
                    new DotModel() { Kind = DotKind.Good, X = -1.5, Y = +3.5, },
                    new DotModel() { Kind = DotKind.Good, X = -8.5, Y = -9.5, },
                    new DotModel() { Kind = DotKind.Good, X = -5.5, Y = -0.5, },
                    new DotModel() { Kind = DotKind.Good, X = -9.9, Y = -0.5, },
                    new DotModel() { Kind = DotKind.Good, X = +7.5, Y = +6.5, },
                    new DotModel() { Kind = DotKind.Good, X = +8.5, Y = +1.5, },
                    new DotModel() { Kind = DotKind.Good, X = -7.5, Y = -4.5, },
                    new DotModel() { Kind = DotKind.Bad,  X = +5.5, Y = +7.5, },
                    new DotModel() { Kind = DotKind.Bad,  X = +2.5, Y = +0.5, },
                },
            };

            // Act
            var game = Game.FromModel(gameModel);

            // Assert
            Assert.IsFalse(game.HasValue);
        }

        /// <summary>
        /// Test that the loading method provides an invalid game if the dots were altered after signing.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveInvalidGameFromModelWithAlteredDot2()
        {
            // Arrange
            var gameModel = new GameModel()
            {
                Id = Guid.Parse("abc2411f-2ef4-4196-a51d-3c215c2494af"),
                Signature = "699D58A4E7C5929A693B4A9C6DF0BA151A19848F4E6A41DCB4722BB7F3BF384A",
                ExpressionStrings = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = -1.5, Y = -1.5, },
                    new DotModel() { Kind = DotKind.Good, X = -1.5, Y = +3.5, },
                    new DotModel() { Kind = DotKind.Good, X = -8.5, Y = -9.5, },
                    new DotModel() { Kind = DotKind.Good, X = -5.5, Y = -0.5, },
                    new DotModel() { Kind = DotKind.Good, X = -8.5, Y = -0.5, },
                    new DotModel() { Kind = DotKind.Good, X = +7.5, Y = +6.5, },
                    new DotModel() { Kind = DotKind.Good, X = +8.5, Y = +1.5, },
                    new DotModel() { Kind = DotKind.Good, X = -7.5, Y = -4.5, },
                    new DotModel() { Kind = DotKind.Bad,  X = +5.5, Y = +7.5, },
                    new DotModel() { Kind = DotKind.Bad,  X = +2.5, Y = +9.9, },
                },
            };

            // Act
            var game = Game.FromModel(gameModel);

            // Assert
            Assert.IsFalse(game.HasValue);
        }
    }
}
