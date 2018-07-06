using Microsoft.VisualStudio.TestTools.UnitTesting;

using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Dots;
using PollyNom.BusinessLogic.Expressions;
using PollyNomTest.Helper;
using System.Collections.Generic;

namespace PollyNomTest
{
    [TestClass]
    public class DotHitting
    {
        private const double startX = -1.0;
        private const double endX = 1.0;
        private const double limits = 1000.0;

        /// <summary>
        /// Tests hitting of a dot at tangential points
        /// </summary>
        [TestMethod]
        public void TrivialBorderCases()
        {
            // Arrange
            GoodDot originDot = new GoodDot(0.0, 0.0);

            IExpression top = new Constant(0.25);
            IExpression bottom = new Constant(-0.25);
            // +1000 * x - 250
            IExpression right = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(+1000.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(250.0)));
            // -1000 * x - 250
            IExpression left = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-1000.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(250.0)));

            IExpression topMiss = new Constant(0.3);
            IExpression bottomMiss = new Constant(-0.3);
            // +1000 * x - 500
            IExpression rightMiss = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(+1000.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(500.0)));
            // -1000 * x - 500
            IExpression leftMiss = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-1000.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(500.0)));

            // Act
            bool topResult = originDot.IsHit(top, null);
            bool bottomResult = originDot.IsHit(bottom, null);
            bool rightResult = originDot.IsHit(right, null);
            bool leftResult = originDot.IsHit(left, null);
            bool topMissResult = originDot.IsHit(topMiss, null);
            bool bottomMissResult = originDot.IsHit(bottomMiss, null);
            bool rightMissResult = originDot.IsHit(rightMiss, null);
            bool leftMissResult = originDot.IsHit(leftMiss, null);

            // Assert
            Assert.IsTrue(DoubleEquality.IsApproximatelyEqual(originDot.Radius, 0.25));

            Assert.IsTrue(topResult);
            Assert.IsTrue(bottomResult);
            Assert.IsTrue(rightResult);
            Assert.IsTrue(leftResult);
            Assert.IsFalse(topMissResult);
            Assert.IsFalse(bottomMissResult);
            Assert.IsFalse(rightMissResult);
            Assert.IsFalse(leftMissResult);
        }

        /// <summary>
        /// Non-trivial cases of singularities
        /// </summary>
        [TestMethod]
        public void Singularity1()
        {
            // Arrange
            GoodDot originDot = new GoodDot(0.0, 0.0);

            IExpression plainOneOverX = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));

            Evaluator evaluator = new Evaluator(plainOneOverX);
            PointListGenerator pointListGenerator = new PointListGenerator(evaluator, DotHitting.startX, DotHitting.endX, DotHitting.limits);

            List<SortedList<double, double>> tupleLists = pointListGenerator.ObtainTuples();

            // Act
            bool plainOneOverXResult = originDot.IsHit(plainOneOverX, tupleLists);

            // Assert
            Assert.IsFalse(plainOneOverXResult);
        }

        /// <summary>
        /// Non-trivial cases of singularities
        /// </summary>
        [TestMethod]
        public void Singularity2()
        {
            // Arrange
            GoodDot originDot = new GoodDot(0.0, 0.0);

            IExpression OneOverXMinus10 = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus,
                    new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(10.0))
                );

            Evaluator evaluator = new Evaluator(OneOverXMinus10);
            PointListGenerator pointListGenerator = new PointListGenerator(evaluator, DotHitting.startX, DotHitting.endX, DotHitting.limits);

            List<SortedList<double, double>> tupleLists = pointListGenerator.ObtainTuples();

            // Act
            bool OneOverXMinus10Result = originDot.IsHit(OneOverXMinus10, tupleLists);

            // Assert
            Assert.IsTrue(OneOverXMinus10Result);
        }
    }
}
