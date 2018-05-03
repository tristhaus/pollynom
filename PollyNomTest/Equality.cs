using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic.Expressions;

namespace PollyNomTest
{
    [TestClass]
    public class Equality
    {
        [TestMethod]
        public void XInvalidPerMethod()
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
        public void XInvalidPerOperator()
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
        public void ConstantEqualPerMethod()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.5);

            Assert.IsTrue(a.Equals(a));
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
        }

        [TestMethod]
        public void ConstantUnequalPerMethod()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.4);

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(b.Equals(a));
        }

        [TestMethod]
        public void ConstantEqualPerOperator()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.5);

            Assert.IsTrue(a==a);
            Assert.IsTrue(a==b);
            Assert.IsTrue(b==a);
        }

        [TestMethod]
        public void ConstantUnequalPerOperator()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.4);

            Assert.IsTrue(a!=b);
            Assert.IsTrue(b!=a);
        }

        [TestMethod]
        public void AddEqualPerOperator()
        {
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void AddUnequalPerOperator()
        {
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        [TestMethod]
        public void AddUnequalSignsPerOperator()
        {
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        [TestMethod]
        public void AddEqualReorderedPerOperator()
        {
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void AddEqualDuplicatePerOperator()
        {
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void AddEqualMixedPerOperator()
        {
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void MultiplyEqualPerOperator()
        {
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void MultiplyUnequalPerOperator()
        {
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        [TestMethod]
        public void MultiplyUnequalSignsPerOperator()
        {
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(1.2)));
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        [TestMethod]
        public void MultiplyEqualReorderedPerOperator()
        {
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void MultiplyEqualDuplicatePerOperator()
        {
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void MultiplyXEqualPerOperator()
        {
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void MultiplyAddEqualPerOperator()
        {
            var a = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX())
                );
            var b = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX())
                );

        //    Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
        }
    }
}
