using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;
using Persistence.Models;

namespace PersistenceTest
{
    /// <summary>
    /// Implements tests to be re-used by <see cref="IGameRepository"/> implementations.
    /// </summary>
    public abstract class GameRepositoryTestBase
    {
        /// <summary>
        /// Gets the implementation used in the tests, unless injected in the call.
        /// </summary>
        protected abstract IGameRepository GameRepository { get; }

        /// <summary>
        /// Tests the roundtrip saving and loading of a game.
        /// </summary>
        protected virtual void ShouldPerformRoundTripSavingLoading()
        {
            // Arrange
            GameModel model = new GameModel()
            {
                Id = Guid.Parse("990a6482-3d44-44d1-8dd2-4eb1f86db0e7"),
                Signature = "someSignature1",
            };
            model.ExpressionStrings = new List<string>() { "exp(x)", "x^2" };
            model.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Good,
                X = 1.2,
                Y = 3.4,
            });
            model.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Bad,
                X = -5.6,
                Y = -7.8,
            });
            const string path = @"F:\temp\test.json";

            // Act
            this.GameRepository.SaveGame(model, path);
            GameModel retrievedModel = this.GameRepository.LoadGame(path);

            // Assert
            Assert.IsTrue(model.Equals(retrievedModel));
        }

        /// <summary>
        /// Tests the overwriting of a game by saving twice.
        /// </summary>
        protected virtual void ShouldAllowOverwriting()
        {
            // Arrange
            GameModel model1 = new GameModel()
            {
                Id = Guid.Parse("990a6482-3d44-44d1-8dd2-aaaaaaaaaaaa"),
                Signature = "someSignature1",
            };
            model1.ExpressionStrings = new List<string>() { "exp(x)", "x^2" };
            model1.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Good,
                X = 1.2,
                Y = 3.4,
            });
            model1.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Bad,
                X = -5.6,
                Y = -7.8,
            });

            GameModel model2 = new GameModel()
            {
                Id = Guid.Parse("990a6482-3d44-44d1-8dd2-bbbbbbbbbbbb"),
                Signature = "someSignature2",
            };
            model2.ExpressionStrings = new List<string>() { "sin(x)", "x^0.5" };
            model2.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Bad,
                X = 1.2,
                Y = 3.4,
            });
            model2.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Good,
                X = -4.4,
                Y = -4.4,
            });
            const string path = @"F:\temp\test.json";

            // Act
            this.GameRepository.SaveGame(model1, path);
            this.GameRepository.SaveGame(model2, path);
            GameModel retrievedModel = this.GameRepository.LoadGame(path);

            // Assert
            Assert.IsTrue(model2.Equals(retrievedModel));
        }

        /// <summary>
        /// Tests the exception thrown when trying to load from a non-existent file.
        /// </summary>
        protected virtual void ShouldThrowForNonExistentFile()
        {
            // Arrange
            const string path = @"F:\temp\neverExists.json";

            // Act
            GameModel retrievedModel = this.GameRepository.LoadGame(path);

            // Assert
            // Exception thrown
        }

        /// <summary>
        /// Tests the exception thrown if the file cannot be deserialized.
        /// </summary>
        /// <param name="specialGameRepository">The specially prepared <see cref="IGameRepository"/> implementation to be used.</param>
        protected virtual void ShouldThrowForNonCompatibleFileContent(IGameRepository specialGameRepository)
        {
            // Arrange
            const string path = @"F:\temp\synchronizedNameOfBadFile.json";

            // Act
            GameModel retrievedModel = specialGameRepository.LoadGame(path);

            // Assert
            // Exception thrown
        }
    }
}
