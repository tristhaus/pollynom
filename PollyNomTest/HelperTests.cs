using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNomTest.Helper;

namespace PollyNomTest
{
    [TestClass]
    public class HelperTests
    {
        [TestMethod]
        public void DoubleMatch()
        {
            // Arrange
            double a = 0.1;
            double b = 0.1;

            // Act
            bool result = DoubleComparer.IsApproximatelyEqual(a, b);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoubleMismatch()
        {
            // Arrange
            double a = 0.1;
            double b = 0.2;

            // Act
            bool result = DoubleComparer.IsApproximatelyEqual(a, b);

            // Assert
            Assert.IsFalse(result);
        }

    }
}
