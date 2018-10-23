using System.Collections.Generic;
using Backend.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to the scoring of hit lists.
    /// </summary>
    [TestClass]
    public class ScoringTest
    {
        /// <summary>
        /// Single list with 3 hits should give 7.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void SimpleCase()
        {
            // Arrange
            List<int> goodList = new List<int>() { 3 };
            List<int> badList = new List<int>(1);

            // Act
            int score = ScoreCalculator.CalculateScore(goodList, badList);

            // Assert
            Assert.IsTrue(score == 7);
        }

        /// <summary>
        /// A list with no hits should give 0.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void NoHitsList()
        {
            // Arrange
            List<int> goodList = new List<int>() { 0 };
            List<int> badList = new List<int>(1);

            // Act
            int score = ScoreCalculator.CalculateScore(goodList, badList);

            // Assert
            Assert.IsTrue(score == 0);
        }

        /// <summary>
        /// An empty list should give 0.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void EmptyList()
        {
            // Arrange
            List<int> goodList = new List<int>(0);
            List<int> badList = new List<int>(1);

            // Act
            int score = ScoreCalculator.CalculateScore(goodList, badList);

            // Assert
            Assert.IsTrue(score == 0);
        }

        /// <summary>
        /// A more complicated case.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void CombinedCase()
        {
            // Arrange
            List<int> goodList = new List<int>() { 4, 0, 2 };
            List<int> badList = new List<int>(1);

            // Act
            int score = ScoreCalculator.CalculateScore(goodList, badList);

            // Assert
            Assert.IsTrue(score == 15 + 3);
        }

        /// <summary>
        /// A more complicated case that also has a bad dot hit.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void CombinedCaseWithBadDots()
        {
            // Arrange
            List<int> goodList = new List<int>() { 4, 0, 2 };
            List<int> badList = new List<int>() { 0, 1, 0 };

            // Act
            int score = ScoreCalculator.CalculateScore(goodList, badList);

            // Assert
            Assert.IsTrue(score == -1);
        }
    }
}
