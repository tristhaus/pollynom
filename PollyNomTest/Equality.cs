using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic.Expressions;

namespace PollyNomTest
{
    [TestClass]
    public class Equality
    {
        [TestMethod]
        public void TestXInvalidPerMethod()
        {
            var Xa = new BaseX();
            var Xb = new BaseX();

            var InvalidA = new InvalidExpression();
            var InvalidB = new InvalidExpression();

            Assert.IsTrue(Xa.Equals(Xb));
            Assert.IsTrue(Xb.Equals(Xa));

            Assert.IsTrue(InvalidA.Equals(InvalidB));
            Assert.IsTrue(InvalidB.Equals(InvalidA));

            Assert.IsFalse(Xa.Equals(InvalidA));
            Assert.IsFalse(InvalidA.Equals(Xa));
        }

        [TestMethod]
        public void TestXInvalidPerOperator()
        {
            var Xa = new BaseX();
            var Xb = new BaseX();

            var InvalidA = new InvalidExpression();
            var InvalidB = new InvalidExpression();

            Assert.IsTrue(Xa == Xb);
            Assert.IsTrue(Xb == Xa);

            Assert.IsTrue(InvalidA == InvalidB);
            Assert.IsTrue(InvalidB == InvalidA);

            Assert.IsFalse(Xa == InvalidA);
            Assert.IsTrue(Xa != InvalidA);

            Assert.IsFalse(InvalidA == Xa);
            Assert.IsTrue(InvalidA != Xa);
        }

        [TestMethod]
        public void TestConstantEqualPerMethod()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.5);

            Assert.IsTrue(a.Equals(a));
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
        }

        [TestMethod]
        public void TestConstantUnequalPerMethod()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.4);

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(b.Equals(a));
        }

        [TestMethod]
        public void TestConstantEqualPerOperator()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.5);

            Assert.IsTrue(a==a);
            Assert.IsTrue(a==b);
            Assert.IsTrue(b==a);
        }

        [TestMethod]
        public void TestConstantUnequalPerOperator()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.4);

            Assert.IsTrue(a!=b);
            Assert.IsTrue(b!=a);
        }
    }
}
