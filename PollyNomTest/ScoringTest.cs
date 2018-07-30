using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using PollyNom.BusinessLogic;

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
        public void SimpleCase()
        {
            // Arrange
            List<int> list = new List<int>() { 3 };

            // Act
            int score = ScoreCalculator.CalculateScore(list);

            // Assert
            Assert.IsTrue(7 == score);
        }

        /// <summary>
        /// A list with no hits should give 0.
        /// </summary>
        [TestMethod]
        public void NoHitsList()
        {
            // Arrange
            List<int> list = new List<int>() { 0 };

            // Act
            int score = ScoreCalculator.CalculateScore(list);

            // Assert
            Assert.IsTrue(0 == score);
        }

        /// <summary>
        /// An empty list should give 0.
        /// </summary>
        [TestMethod]
        public void EmptyList()
        {
            // Arrange
            List<int> list = new List<int>(0);

            // Act
            int score = ScoreCalculator.CalculateScore(list);

            // Assert
            Assert.IsTrue(0 == score);
        }

        /// <summary>
        /// A more complicated case.
        /// </summary>
        [TestMethod]
        public void CombinedCase()
        {
            // Arrange
            List<int> list = new List<int>() { 4, 0, 2 };

            // Act
            int score = ScoreCalculator.CalculateScore(list);

            // Assert
            Assert.IsTrue(15 + 3 == score);
        }
    }
}
