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

﻿using Backend.BusinessLogic.Expressions;
using Backend.BusinessLogic.Expressions.SingleArgumentFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void XInvalidPerMethod()
        {
            var xA = new BaseX();
            var xB = new BaseX();

            var invalidA = new InvalidExpression();
            var invalidB = new InvalidExpression();

            Assert.IsTrue(xA.Equals(xB));
            Assert.IsTrue(xB.Equals(xA));

            Assert.IsTrue(invalidA.Equals(invalidB));
            Assert.IsTrue(invalidB.Equals(invalidA));

            Assert.IsFalse(xA.Equals(invalidA));
            Assert.IsFalse(invalidA.Equals(xA));
        }

        /// <summary>
        /// Tests about X and invalid expressions, using the operator.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void XInvalidPerOperator()
        {
            var xA = new BaseX();
            var xB = new BaseX();

            var invalidA = new InvalidExpression();
            var invalidB = new InvalidExpression();

            Assert.IsTrue(xA == xB);
            Assert.IsTrue(xB == xA);

            Assert.IsTrue(invalidA == invalidB);
            Assert.IsTrue(invalidB == invalidA);

            Assert.IsFalse(xA == invalidA);
            Assert.IsTrue(xA != invalidA);

            Assert.IsFalse(invalidA == xA);
            Assert.IsTrue(invalidA != xA);
        }

        /// <summary>
        /// Tests about constant expressions, using the method.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void MultiplyAddEqualPerOperator()
        {
            // (X + 4.2) / (X)
            var a = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));

            // / (X) * (X + 4.2)
            var b = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))));

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        /// <summary>
        /// Tests about multiply and addition expressions, using the operator.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void MultiplyAddUnequalPerOperator()
        {
            // (X + 4.2) / (X)
            var a = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));

            // (-X + 4.2) / (X)
            var b = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.2)))),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));

            Assert.IsFalse(a == b);
            Assert.IsFalse(b == a);
        }

        /// <summary>
        /// Tests about multiply expressions involving reordering, using the method.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void MultiplySignAndOrderPerMethod()
        {
            var aa = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4)));
            var a = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4)),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()));
            var b = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.5)),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()));

            Assert.IsTrue(aa.Equals(a));
            Assert.IsFalse(aa.Equals(b));
            Assert.IsFalse(a.Equals(b));
        }

        /// <summary>
        /// Tests about power expressions, involving reordering, using the operator.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void PowerEqualPerOperator()
        {
            var exponentA = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4)));
            var basisA = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var exponentB = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4)),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()));
            var basisB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            // (X - 1.0) ^ (X / 0.4)
            var powerA = new Power(basisA, exponentA);

            // (X - 1.0) ^ (/ 0.4 * X)
            var powerB = new Power(basisB, exponentB);

            Assert.IsTrue(powerA == powerB);
            Assert.IsTrue(powerB == powerA);
        }

        /// <summary>
        /// Tests about power expressions, involving different constants, using the operator.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void PowerUnequalPerOperator()
        {
            var exponentA = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.3)));
            var basisA = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            var exponentB = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(0.4)));
            var basisB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(1.0)));

            // (X - 1.0) ^ (X / 0.3)
            var powerA = new Power(basisA, exponentA);

            // (X - 1.0) ^ (X / 0.4)
            var powerB = new Power(basisB, exponentB);

            Assert.IsTrue(powerA != powerB);
            Assert.IsTrue(powerB != powerA);
        }

        /// <summary>
        /// Tests about power expressions, involving different constants, using the method.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
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
        /// Tests about exponential function, using the method.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ExponentialPerMethod()
        {
            var argumentA = new BaseX();
            var argumentAA = new BaseX();
            var argumentB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new Power(new BaseX(), new Constant(2.0))));

            // exp(x)
            var expressionA = new Exponential(argumentA);

            // exp(x)
            var expressionAA = new Exponential(argumentAA);

            // exp(-x^2)
            var expressionB = new Exponential(argumentB);

            Assert.IsTrue(expressionA.Equals(expressionAA));
            Assert.IsFalse(expressionB.Equals(expressionA));
            Assert.IsFalse(expressionB.Equals(expressionAA));
        }

        /// <summary>
        /// Tests about exponential function, using the operator.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void ExponentialPerOperator()
        {
            var argumentA = new BaseX();
            var argumentAA = new BaseX();
            var argumentB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new Power(new BaseX(), new Constant(2.0))));

            // exp(x)
            var expressionA = new Exponential(argumentA);

            // exp(x)
            var expressionAA = new Exponential(argumentAA);

            // exp(-x^2)
            var expressionB = new Exponential(argumentB);

            Assert.IsTrue(expressionA == expressionAA);
            Assert.IsFalse(expressionB == expressionA);
            Assert.IsTrue(expressionB != expressionAA);
        }

        /// <summary>
        /// Tests about logarithm function, using the method.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void LogarithmPerMethod()
        {
            var argumentA = new BaseX();
            var argumentAA = new BaseX();
            var argumentB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Power(new BaseX(), new Constant(2.0))));

            // ln(x)
            var expressionA = new NaturalLogarithm(argumentA);

            // ln(x)
            var expressionAA = new NaturalLogarithm(argumentAA);

            // ln(x^2)
            var expressionB = new NaturalLogarithm(argumentB);

            Assert.IsTrue(expressionA.Equals(expressionAA));
            Assert.IsFalse(expressionB.Equals(expressionA));
            Assert.IsFalse(expressionB.Equals(expressionAA));
        }

        /// <summary>
        /// Tests about logarithm function, using the operator.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void LogarithmPerOperator()
        {
            var argumentA = new BaseX();
            var argumentAA = new BaseX();
            var argumentB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Power(new BaseX(), new Constant(2.0))));

            // ln(x)
            var expressionA = new NaturalLogarithm(argumentA);

            // ln(x)
            var expressionAA = new NaturalLogarithm(argumentAA);

            // ln(x^2)
            var expressionB = new NaturalLogarithm(argumentB);

            Assert.IsTrue(expressionA == expressionAA);
            Assert.IsFalse(expressionB == expressionA);
            Assert.IsTrue(expressionB != expressionAA);
        }

        /// <summary>
        /// Tests about sine function, using both the operator and the method.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void SinePerOperatorAndPerMethod()
        {
            var argumentA = new BaseX();
            var argumentAA = new BaseX();
            var argumentB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Power(new BaseX(), new Constant(2.0))));

            // sin(x)
            var expressionA = new Sine(argumentA);

            // sin(x)
            var expressionAA = new Sine(argumentAA);

            // sin(x^2)
            var expressionB = new Sine(argumentB);

            Assert.IsTrue(expressionA == expressionAA);
            Assert.IsFalse(expressionB == expressionA);
            Assert.IsTrue(expressionB != expressionAA);

            Assert.IsTrue(expressionA.Equals(expressionAA));
            Assert.IsFalse(expressionB.Equals(expressionA));
            Assert.IsFalse(expressionB.Equals(expressionAA));
        }

        /// <summary>
        /// Tests about cosine function, using both the operator and the method.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void CosinePerOperatorAndPerMethod()
        {
            var argumentA = new BaseX();
            var argumentAA = new BaseX();
            var argumentB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Power(new BaseX(), new Constant(2.0))));

            // cos(x)
            var expressionA = new Cosine(argumentA);

            // cos(x)
            var expressionAA = new Cosine(argumentAA);

            // cos(x^2)
            var expressionB = new Cosine(argumentB);

            Assert.IsTrue(expressionA == expressionAA);
            Assert.IsFalse(expressionB == expressionA);
            Assert.IsTrue(expressionB != expressionAA);

            Assert.IsTrue(expressionA.Equals(expressionAA));
            Assert.IsFalse(expressionB.Equals(expressionA));
            Assert.IsFalse(expressionB.Equals(expressionAA));
        }

        /// <summary>
        /// Tests about tangent function, using both the operator and the method.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void TangentPerOperatorAndPerMethod()
        {
            var argumentA = new BaseX();
            var argumentAA = new BaseX();
            var argumentB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Power(new BaseX(), new Constant(2.0))));

            // tan(x)
            var expressionA = new Tangent(argumentA);

            // tan(x)
            var expressionAA = new Tangent(argumentAA);

            // tan(x^2)
            var expressionB = new Tangent(argumentB);

            Assert.IsTrue(expressionA == expressionAA);
            Assert.IsFalse(expressionB == expressionA);
            Assert.IsTrue(expressionB != expressionAA);

            Assert.IsTrue(expressionA.Equals(expressionAA));
            Assert.IsFalse(expressionB.Equals(expressionA));
            Assert.IsFalse(expressionB.Equals(expressionAA));
        }

        /// <summary>
        /// Tests about absolute value function, using both the operator and the method.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void AbsoluteValuePerOperatorAndPerMethod()
        {
            var argumentA = new BaseX();
            var argumentAA = new BaseX();
            var argumentB = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Power(new BaseX(), new Constant(2.0))));

            // abs(x)
            var expressionA = new AbsoluteValue(argumentA);

            // abs(x)
            var expressionAA = new AbsoluteValue(argumentAA);

            // abs(x^2)
            var expressionB = new AbsoluteValue(argumentB);

            Assert.IsTrue(expressionA == expressionAA);
            Assert.IsFalse(expressionB == expressionA);
            Assert.IsTrue(expressionB != expressionAA);

            Assert.IsTrue(expressionA.Equals(expressionAA));
            Assert.IsFalse(expressionB.Equals(expressionA));
            Assert.IsFalse(expressionB.Equals(expressionAA));
        }

        /// <summary>
        /// Tests about nontrivial equality, involving recursion (for reordered expression) and hash map challenges.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void NontrivialMultiplyAddMix()
        {
            // ( -2.1 * (X + 3.1) ) + 1.1
            var innerBracketA = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.1)));

            var outerBracketA = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.1)),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, innerBracketA));

            var exprA = new Add(
                            new Add.AddExpression(Add.AddExpression.Signs.Plus, outerBracketA),
                            new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.1)));

            // ( (3.1 + X) * -2.1 ) + 1.1
            var innerBracketB = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.1)),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()));

            var outerBracketB = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, innerBracketB),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.1)));

            var exprB = new Add(
                            new Add.AddExpression(Add.AddExpression.Signs.Plus, outerBracketB),
                            new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.1)));

            Assert.IsTrue(exprA.Equals(exprB));
            Assert.IsTrue(exprB.Equals(exprA));
        }
    }
}
