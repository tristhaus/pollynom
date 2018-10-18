using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;
using Persistence.Models;
using PersistenceTest.Helper;

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
            GameModel model = new GameModel();
            model.DotModels.Add(new DotModel()
            {
                X = 1.2,
                Y = 3.4,
            });
            model.DotModels.Add(new DotModel()
            {
                X = -5.6,
                Y = -7.8,
            });
            const string path = @"C:\temp\test.json";

            // Act
            this.GameRepository.SaveGame(model, path);
            GameModel retrievedModel = this.GameRepository.LoadGame(path);

            // Assert
            Assert.IsTrue(GameModelEqualityChecker.AreApproximatelyEqual(model, retrievedModel));
        }

        /// <summary>
        /// Tests the overwriting of a game by saving twice.
        /// </summary>
        protected virtual void ShouldAllowOverwriting()
        {
            // Arrange
            GameModel model1 = new GameModel();
            model1.DotModels.Add(new DotModel()
            {
                X = 1.2,
                Y = 3.4,
            });
            model1.DotModels.Add(new DotModel()
            {
                X = -5.6,
                Y = -7.8,
            });

            GameModel model2 = new GameModel();
            model2.DotModels.Add(new DotModel()
            {
                X = 1.2,
                Y = 3.4,
            });
            model2.DotModels.Add(new DotModel()
            {
                X = -4.4,
                Y = -4.4,
            });
            const string path = @"C:\temp\test.json";

            // Act
            this.GameRepository.SaveGame(model1, path);
            this.GameRepository.SaveGame(model2, path);
            GameModel retrievedModel = this.GameRepository.LoadGame(path);

            // Assert
            Assert.IsTrue(GameModelEqualityChecker.AreApproximatelyEqual(model2, retrievedModel));
        }

        /// <summary>
        /// Tests the exception thrown when trying to load from a non-existent file.
        /// </summary>
        protected virtual void ShouldThrowForNonExistentFile()
        {
            // Arrange
            const string path = @"C:\temp\neverExists.json";

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
            const string path = @"C:\temp\synchronizedNameOfBadFile.json";

            // Act
            GameModel retrievedModel = specialGameRepository.LoadGame(path);

            // Assert
            // Exception thrown
        }
    }
}
