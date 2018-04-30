using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic;

namespace PollyNomTest
{
    [TestClass]
    public class Outputting
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            Expression konst = new Constant(1.0/3.0);
            Expression unknown = new BaseX();
            Expression expr = new Multiply(konst, unknown);

            // Act
            string result = expr.Print().HasValue() ? expr.Print().Value() : "invalid";

            // Assert
            Assert.AreEqual<string>($"{1.0/3.0}*x", result);
        }
    }
}
