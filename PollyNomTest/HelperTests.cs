/*
 * This file is part of PollyNom.
 * 
 * PollyNom is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * PollyNom is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with PollyNom.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
