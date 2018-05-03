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

            Assert.IsTrue(a == a);
            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void ConstantUnequalPerOperator()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.4);

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
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
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2))))
                );

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void MultiplyAddUnequalPerOperator()
        {
            var a = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX())
                );
            var b = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX())
                );

            Assert.IsFalse(a == b);
            Assert.IsFalse(b == a);
        }

        [TestMethod]
        public void MultiplySignAndOrderPerMethod()
        {
            var aa = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4))
                );
            var a = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4)),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX())
                );
            var b = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.5)),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX())
                );

            Assert.IsTrue(aa.Equals(a));
            Assert.IsFalse(aa.Equals(b));
            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod]
        public void PowerEqualPerOperator()
        {
            var exponentA = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4))
                );
            var basisA = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var exponentB = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4)),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX())
                );
            var basisB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var powerA = new Power(basisA, exponentA);
            var powerB = new Power(basisB, exponentB);

            Assert.IsTrue(powerA == powerB);
            Assert.IsTrue(powerB == powerA);
        }

        [TestMethod]
        public void PowerUnequalPerOperator()
        {
            var exponentA = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.3))
                );
            var basisA = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var exponentB = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4))
                );
            var basisB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var powerA = new Power(basisA, exponentA);
            var powerB = new Power(basisB, exponentB);

            Assert.IsTrue(powerA != powerB);
            Assert.IsTrue(powerB != powerA);
        }

        [TestMethod]
        public void PowerPerMethod()
        {
            var exponentA = new BaseX();
            var basisA = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var exponentAA = new BaseX();
            var basisAA = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var exponentB = new Constant(1.4);
            var basisB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var powerA = new Power(basisA, exponentA);
            var powerAA = new Power(basisAA, exponentAA);
            var powerB = new Power(basisB, exponentB);

            Assert.IsTrue(powerA.Equals(powerAA));
            Assert.IsFalse(powerB.Equals(powerA));
            Assert.IsFalse(powerB.Equals(powerAA));
        }
    }
}
