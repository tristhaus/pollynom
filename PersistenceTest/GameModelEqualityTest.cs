using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence.Models;

namespace PersistenceTest
{
    /// <summary>
    /// Collects test related to the <see cref="GameModel"/> equality check helper.
    /// </summary>
    [TestClass]
    public class GameModelEqualityTest
    {
        /// <summary>
        /// Check two identical game models.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveTrueForIdenticalGameModels()
        {
            // Arrange
            GameModel model1 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "b", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                    new DotModel() { Kind = DotKind.Bad, X = -5.6, Y = -7.8, },
                }
            };

            GameModel model2 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "b", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                    new DotModel() { Kind = DotKind.Bad, X = -5.6, Y = -7.8, },
                }
            };

            // Act
            bool result = model1.Equals(model2);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Check two models differing in number of dots contained.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveFalseForDifferentNumberOfDots()
        {
            // Arrange
            GameModel model1 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "b", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                    new DotModel() { Kind = DotKind.Bad, X = -5.6, Y = -7.8, },
                }
            };

            GameModel model2 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "b", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                }
            };

            // Act
            bool result = model1.Equals(model2);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Check two models differing in a dot contained.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveFalseForDifferentDots()
        {
            // Arrange
            GameModel model1 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "b", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                    new DotModel() { Kind = DotKind.Bad, X = -5.6, Y = -7.8, },
                }
            };

            GameModel model2 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "b", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                    new DotModel() { Kind = DotKind.Bad, X = -5.6, Y = -9.9, },
                }
            };

            // Act
            bool result = model1.Equals(model2);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Check two game models differing in terms of dot model order.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveFalseForGameModelsDifferingInOrder()
        {
            // Arrange
            GameModel model1 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "b", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                    new DotModel() { Kind = DotKind.Bad, X = -5.6, Y = -7.8, },
                }
            };

            GameModel model2 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "b", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Bad, X = -5.6, Y = -7.8, },
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                }
            };

            // Act
            bool result = model1.Equals(model2);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Check two game models differing in terms of expressions contained.
        /// </summary>
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ShouldGiveFalseForGameModelsDifferingInExpressions()
        {
            // Arrange
            GameModel model1 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "c", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                    new DotModel() { Kind = DotKind.Bad, X = -5.6, Y = -7.8, },
                }
            };

            GameModel model2 = new GameModel()
            {
                ExpressionStrings = new List<string>() { "a", "b", },
                DotModels = new List<DotModel>()
                {
                    new DotModel() { Kind = DotKind.Good, X = 1.2, Y = 3.4, },
                    new DotModel() { Kind = DotKind.Bad, X = -5.6, Y = -7.8, },
                }
            };

            // Act
            bool result = model1.Equals(model2);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
