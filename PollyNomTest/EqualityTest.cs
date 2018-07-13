using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic.Expressions;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests for equality of instances of <see cref="IExpression"/>.
    /// </summary>
    [TestClass]
    public class EqualityTest
    {
        /// <summary>
        /// Tests about X and invalid expressions, using the method.
        /// </summary>
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

        /// <summary>
        /// Tests about X and invalid expressions, using the operator.
        /// </summary>
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

        /// <summary>
        /// Tests about constant expressions, using the method.
        /// </summary>
        [TestMethod]
        public void ConstantEqualPerMethod()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.5);

            Assert.IsTrue(a.Equals(a));
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
        }

        /// <summary>
        /// Tests about constant expressions, using the method.
        /// </summary>
        [TestMethod]
        public void ConstantUnequalPerMethod()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.4);

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(b.Equals(a));
        }

        /// <summary>
        /// Tests about constant expressions, using the operator.
        /// </summary>
        [TestMethod]
        public void ConstantEqualPerOperator()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.5);
            var sameA = a;

            Assert.IsTrue(a == sameA);
            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about constant expressions, using the operator.
        /// </summary>
        [TestMethod]
        public void ConstantUnequalPerOperator()
        {
            var a = new Constant(0.5);
            var b = new Constant(0.4);

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        /// <summary>
        /// Tests about addition expressions, using the operator.
        /// </summary>
        [TestMethod]
        public void AddEqualPerOperator()
        {
            // 0.5 + 0.3 + 1.2
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about addition expressions, using the operator.
        /// </summary>
        [TestMethod]
        public void AddUnequalPerOperator()
        {
            // 0.5 + 0.3 + 1.2
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        /// <summary>
        /// Tests about addition expressions involving different <see cref="Add.AddExpression.Signs"/>, using the operator.
        /// </summary>
        [TestMethod]
        public void AddUnequalSignsPerOperator()
        {
            // 0.5 + 0.3 - 1.2
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.2)));
            // -0.5 + 0.3 + 1.2
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        /// <summary>
        /// Tests about addition expressions involving different order of terms, using the operator.
        /// </summary>
        [TestMethod]
        public void AddEqualReorderedPerOperator()
        {
            // 0.5 + 0.3 + 1.2
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            // 0.3 + 0.5 + 1.2
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.5)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about addition expressions involving duplicated terms, using the operator.
        /// </summary>
        [TestMethod]
        public void AddEqualDuplicatePerOperator()
        {
            // 0.3 + 0.3 + 1.2
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about addition expressions involving duplicated terms of different classes, using the operator.
        /// </summary>
        [TestMethod]
        public void AddEqualMixedPerOperator()
        {
            var a = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));
            var b = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(0.3)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about multiply expressions, using the operator.
        /// </summary>
        [TestMethod]
        public void MultiplyEqualPerOperator()
        {
            // 0.5 * 0.3 * 1.2
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about multiply expressions, using the operator.
        /// </summary>
        [TestMethod]
        public void MultiplyUnequalPerOperator()
        {
            // 0.5 * 0.3 * 1.2
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            // 0.3 * 0.3 * 1.2
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        /// <summary>
        /// Tests about multiply expressions, using the operator.
        /// </summary>
        [TestMethod]
        public void MultiplyUnequalSignsPerOperator()
        {
            // 0.5 * 0.3 / 1.2
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(1.2)));
            // / 0.5 * 0.3 * 1.2
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        /// <summary>
        /// Tests about multiply expressions involving different order of terms, using the operator.
        /// </summary>
        [TestMethod]
        public void MultiplyEqualReorderedPerOperator()
        {
            // 0.5 * 0.3 * 1.2
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            // 0.3 * 0.5 * 1.2
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.5)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about multiply expressions involving duplicated terms, using the operator.
        /// </summary>
        [TestMethod]
        public void MultiplyEqualDuplicatePerOperator()
        {
            // 0.3 * 0.3 * 1.2
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about multiply expressions involving duplicated terms of different classes, using the operator.
        /// </summary>
        [TestMethod]
        public void MultiplyXEqualPerOperator()
        {
            // 0.3 * X * 1.2
            var a = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));
            // X * 0.3 * 1.2
            var b = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(0.3)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.2)));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about multiply and addition expressions involving reordering, using the operator.
        /// </summary>
        [TestMethod]
        public void MultiplyAddEqualPerOperator()
        {
            // (X + 4.2) / (X)
            var a = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX())
                );
            // / (X) * (X + 4.2)
            var b = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2))))
                );

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about multiply and addition expressions, using the operator.
        /// </summary>
        [TestMethod]
        public void MultiplyAddUnequalPerOperator()
        {
            // (X + 4.2) / (X)
            var a = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX())
                );
            // (-X + 4.2) / (X)
            var b = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX())
                );

            Assert.IsFalse(a == b);
            Assert.IsFalse(b == a);
        }

        /// <summary>
        /// Tests about multiply expressions involving reordering, using the method.
        /// </summary>
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

        /// <summary>
        /// Test about power expressions, involving reordering, using the operator.
        /// </summary>
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

            // (X - 1.0) ^ (X / 0.4)
            var powerA = new Power(basisA, exponentA);
            // (X - 1.0) ^ (/ 0.4 * X)
            var powerB = new Power(basisB, exponentB);

            Assert.IsTrue(powerA == powerB);
            Assert.IsTrue(powerB == powerA);
        }

        /// <summary>
        /// Test about power expressions, involving different constants, using the operator.
        /// </summary>
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

            // (X - 1.0) ^ (X / 0.3)
            var powerA = new Power(basisA, exponentA);
            // (X - 1.0) ^ (X / 0.4)
            var powerB = new Power(basisB, exponentB);

            Assert.IsTrue(powerA != powerB);
            Assert.IsTrue(powerB != powerA);
        }

        /// <summary>
        /// Test about power expressions, involving different constants, using the ,method.
        /// </summary>
        [TestMethod]
        public void PowerPerMethod()
        {
            var exponentA = new BaseX();
            var basisA = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var exponentAA = new BaseX();
            var basisAA = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var exponentB = new Constant(1.4);
            var basisB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            // (X - 1.0) ^ X
            var powerA = new Power(basisA, exponentA);
            // (X - 1.0) ^ X
            var powerAA = new Power(basisAA, exponentAA);
            // (X - 1.0) ^ (1.4)
            var powerB = new Power(basisB, exponentB);

            Assert.IsTrue(powerA.Equals(powerAA));
            Assert.IsFalse(powerB.Equals(powerA));
            Assert.IsFalse(powerB.Equals(powerAA));
        }

        /// <summary>
        /// Test about nontrivial equality, involving recursion (for reordered expression) and hash map challenges.
        /// </summary>
        [TestMethod]
        public void NontrivialMultiplyAddMix()
        {
            // ( -2.1 * (X + 3.1) ) + 1.1
            var exprA = new Add(
                            new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.1)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.1)))))
                            ),
                            new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.1))
                        );

            // ( (X + 3.1) * -2.1 ) + 1.1
            var exprB = new Add(
                            new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.1)))), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.1)))
                            ),
                            new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.1))
                        );

            Assert.IsTrue(exprA.Equals(exprB));
            Assert.IsTrue(exprB.Equals(exprA));
        }
    }
}
