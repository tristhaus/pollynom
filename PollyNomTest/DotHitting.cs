using Microsoft.VisualStudio.TestTools.UnitTesting;

using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Dots;
using PollyNom.BusinessLogic.Expressions;
using PollyNomTest.Helper;


namespace PollyNomTest
{
    [TestClass]
    public class DotHitting
    {
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
            bool topResult = originDot.IsHit(top);
            bool bottomResult = originDot.IsHit(bottom);
            bool rightResult = originDot.IsHit(right);
            bool leftResult = originDot.IsHit(left);
            bool topMissResult = originDot.IsHit(topMiss);
            bool bottomMissResult = originDot.IsHit(bottomMiss);
            bool rightMissResult = originDot.IsHit(rightMiss);
            bool leftMissResult = originDot.IsHit(leftMiss);

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
        public void Singularities()
        {
            // Arrange
            GoodDot originDot = new GoodDot(0.0, 0.0);

            IExpression plainOneOverX = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));
            IExpression OneOverXMinus10 = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus,
                    new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(10.0))
                );

            // Act
            bool plainOneOverXResult = originDot.IsHit(plainOneOverX);
            bool OneOverXMinus10Result = originDot.IsHit(OneOverXMinus10);

            // Assert
            Assert.IsFalse(plainOneOverXResult);
            Assert.IsTrue(OneOverXMinus10Result);
        }

    }
}
