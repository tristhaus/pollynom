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

        [TestMethod]
        public void EmptyList()
        {
            // Arrange
            List<int> list = new List<int>() { 0 };

            // Act
            int score = ScoreCalculator.CalculateScore(list);

            // Assert
            Assert.IsTrue(0 == score);
        }

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
