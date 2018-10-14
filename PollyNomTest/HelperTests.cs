using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestInfrastructure;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to internal test tools.
    /// </summary>
    [TestClass]
    public class HelperTests
    {
        /// <summary>
        /// Tests that two instances of <see cref="double"/> match.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void DoubleMatch()
        {
            // Arrange
            double a = 0.1;
            double b = 0.1;

            // Act
            bool result = DoubleEquality.IsApproximatelyEqual(a, b);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that two instances of <see cref="double"/> do not match.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void DoubleMismatch()
        {
            // Arrange
            double a = 0.1;
            double b = 0.2;

            // Act
            bool result = DoubleEquality.IsApproximatelyEqual(a, b);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
