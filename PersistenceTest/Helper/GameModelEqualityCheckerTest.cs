using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence.Models;

namespace PersistenceTest.Helper
{
    [TestClass]
    public class GameModelEqualityCheckerTest
    {
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveTrueForIdenticalGameModels()
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
                X = -5.6,
                Y = -7.8,
            });

            // Act
            bool result = GameModelEqualityChecker.AreApproximatelyEqual(model1, model2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveFalseForDifferentNumberOfDots()
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

            // Act
            bool result = GameModelEqualityChecker.AreApproximatelyEqual(model1, model2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveFalseForDifferentDots()
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
                X = -5.6,
                Y = -9.9,
            });

            // Act
            bool result = GameModelEqualityChecker.AreApproximatelyEqual(model1, model2);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
