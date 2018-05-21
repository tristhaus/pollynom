using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Expressions;
using PollyNomTest.Helper;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to the parsing of human-readable strings to expressions.
    /// </summary>
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void SimpleTests()
        {
            // Arrange
            Parser parser = new Parser();
            string x = "X";
            string y = "Y";
            string number = " 030.500 ";
            string invalidNumber = "030.50.0";
            string bracedX = "(X)";
            string doubleBracedX = "((X))";

            // Act
            IExpression exprX = parser.Parse(x);
            IExpression exprInvalid = parser.Parse(y);
            IExpression exprNumber = parser.Parse(number);
            IExpression exprInvalidNumber = parser.Parse(invalidNumber);
            IExpression exprBracedX = parser.Parse(bracedX);
            IExpression exprDoubleBracedX = parser.Parse(doubleBracedX);

            // Assert
            Assert.IsTrue(exprX.Equals(new BaseX()));
            Assert.IsTrue(exprInvalid.Equals(new InvalidExpression()));
            Assert.IsTrue(exprNumber.Equals(new Constant(30.5)));
            Assert.IsTrue(exprInvalidNumber.Equals(new InvalidExpression()));
            Assert.IsTrue(exprBracedX.Equals(new BaseX()));
            Assert.IsTrue(exprDoubleBracedX.Equals(new BaseX()));
        }

        /// <summary>
        /// Test whether single addition terms evaluate to Constant, regardless of sign.
        /// </summary>
        [TestMethod]
        public void SingleAdditionTermAsConstant()
        {
            // Arrange
            Parser parser = new Parser();
            string positive = "+1.1";
            string negative = "-2.2";

            // Act
            IExpression exprPositive = parser.Parse(positive);
            IExpression exprNegative = parser.Parse(negative);

            // Assert
            Assert.IsTrue(exprPositive.Equals(new Constant(+1.1)));
            Assert.IsTrue(exprNegative.Equals(new Constant(-2.2)));
        }

        [TestMethod]
        public void SingleXasAdd()
        {
            // Arrange
            Parser parser = new Parser();
            string signedX = "-x";

            // Act
            IExpression exprSignedX = parser.Parse(signedX);

            // Assert
            Assert.IsTrue(exprSignedX.Equals(new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new BaseX()))));
        }

        [TestMethod]
        public void SimpleAddition01()
        {
            // Arrange
            Parser parser = new Parser();
            string TwoAdd = "+2.0+3.0";
            var expectedTwoAdd = new Add(new Constant(2.0), new Constant(3.0));

            // Act
            IExpression exprTwoAdd = parser.Parse(TwoAdd);

            // Assert
            Assert.IsTrue(exprTwoAdd.Equals(expectedTwoAdd));
        }

        [TestMethod]
        public void SimpleAddition02()
        {
            // Arrange
            Parser parser = new Parser();
            string TwoSubtract = "-2.0-3.0";
            var expectedTwoSubtract = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(-2.0)), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(3.0)));

            // Act
            IExpression exprTwoSubtract = parser.Parse(TwoSubtract);

            // Assert
            Assert.IsTrue(expectedTwoSubtract.Equals(exprTwoSubtract));
        }

        [TestMethod]
        public void SimpleAddition03()
        {
            // Arrange
            Parser parser = new Parser();
            string TwoAddBracketed = "(2.0)+(3.0)";
            var expectedTwoAddBracketed = new Add(new Constant(2.0), new Constant(3.0));

            // Act
            IExpression exprTwoAddBracketed = parser.Parse(TwoAddBracketed);

            // Assert
            Assert.IsTrue(exprTwoAddBracketed.Equals(expectedTwoAddBracketed));
        }

        [TestMethod]
        public void SimpleAddition04()
        {
            // Arrange
            Parser parser = new Parser();
            string ThreeAdd = "2.0+3.0-x";
            var expectedThreeAdd = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(2.0)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.0)), new Add.AddExpression(Add.AddExpression.Signs.Minus, new BaseX()));

            // Act
            IExpression exprThreeAdd = parser.Parse(ThreeAdd);

            // Assert
            Assert.IsTrue(exprThreeAdd.Equals(expectedThreeAdd));
        }

        [TestMethod]
        public void SimpleAddition05()
        {
            // Arrange
            Parser parser = new Parser();
            string ThreeAddWithBrackets = "2.0-(3.0+x)+1.0";
            var expectedThreeAddWithBrackets = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(2.0)),
                new Add.AddExpression(Add.AddExpression.Signs.Minus,
                    new Add(
                        new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.0)),
                        new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX())
                        )
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0))
                );

            // Act
            IExpression exprThreeAddWithBrackets = parser.Parse(ThreeAddWithBrackets);

            // Assert
            Assert.IsTrue(exprThreeAddWithBrackets.Equals(expectedThreeAddWithBrackets));
        }

        [TestMethod]
        public void SimpleMultiplication01()
        {
            // Arrange
            Parser parser = new Parser();
            string TwoMultiply = "2.0*3.0";
            var expectedTwoMultiply = new Multiply(new Constant(2.0), new Constant(3.0));

            // Act
            IExpression exprTwoMultiply = parser.Parse(TwoMultiply);

            // Assert
            Assert.IsTrue(exprTwoMultiply.Equals(expectedTwoMultiply));
        }

        [TestMethod]
        public void SimpleMultiplication02()
        {
            // Arrange
            Parser parser = new Parser();
            string TwoDivide = "2.0/3.0";
            var expectedTwoDivide = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(3.0)));

            // Act
            IExpression exprTwoDivide = parser.Parse(TwoDivide);

            // Assert
            Assert.IsTrue(expectedTwoDivide.Equals(exprTwoDivide));
        }

        [TestMethod]
        public void SimpleMultiplication03()
        {
            // Arrange
            Parser parser = new Parser();
            string TwoMultiplyBracketed = "(2.0)*(3.0)";
            var expectedTwoMultiplyBracketed = new Multiply(new Constant(2.0), new Constant(3.0));

            // Act
            IExpression exprTwoMultiplyBracketed = parser.Parse(TwoMultiplyBracketed);

            // Assert
            Assert.IsTrue(exprTwoMultiplyBracketed.Equals(expectedTwoMultiplyBracketed));
        }

        [TestMethod]
        public void SimpleMultiplication04()
        {
            // Arrange
            Parser parser = new Parser();
            string ThreeMultiply = "2.0*3.0/x";
            var expectedThreeMultiply = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(3.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));

            // Act
            IExpression exprThreeMultiply = parser.Parse(ThreeMultiply);

            // Assert
            Assert.IsTrue(exprThreeMultiply.Equals(expectedThreeMultiply));
        }

        [TestMethod]
        public void SimpleMultiplication05()
        {
            // Arrange
            Parser parser = new Parser();
            string ThreeMultiplyWithBrackets = "2.0/(3.0*x)*1.0";
            var expectedThreeMultiplyWithBrackets = new Multiply(
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide,
                    new Multiply(
                        new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(3.0)),
                        new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX())
                        )
                    ),
                new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(1.0))
                );

            // Act
            IExpression exprThreeMultiplyWithBrackets = parser.Parse(ThreeMultiplyWithBrackets);

            // Assert
            Assert.IsTrue(exprThreeMultiplyWithBrackets.Equals(expectedThreeMultiplyWithBrackets));
        }

        [TestMethod]
        public void AddMultiplyMix01()
        {
            // Arrange
            Parser parser = new Parser();
            string ThreeTerms = "2.0*x+1.0";
            var expectedThreeTerms = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0))
                );

            // Act
            IExpression exprThreeTerms = parser.Parse(ThreeTerms);

            // Assert
            Assert.IsTrue(exprThreeTerms.Equals(expectedThreeTerms));
        }

        [TestMethod]
        public void AddMultiplyMix02()
        {
            // Arrange
            Parser parser = new Parser();
            string ThreeTerms = "-2.0*x+1.0";
            var expectedThreeTerms = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0))
                );

            // Act
            IExpression exprThreeTerms = parser.Parse(ThreeTerms);

            // Assert
            Assert.IsTrue(exprThreeTerms.Equals(expectedThreeTerms));
        }

        [TestMethod]
        public void AddMultiplyMix03()
        {
            // Arrange
            Parser parser = new Parser();
            string ThreeTerms = "-2.1*(x+3.1)+1.1";
            var expectedThreeTerms = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.1)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.1)))))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.1))
                );

            // Act
            IExpression exprThreeTerms = parser.Parse(ThreeTerms);

            // Assert
            Assert.IsTrue(exprThreeTerms.Equals(expectedThreeTerms));
        }

        [TestMethod]
        public void AddMultiplyMix04()
        {
            // Arrange
            Parser parser = new Parser();
            string ThreeTerms = "(x+3.1)*-2.1+1.1";
            var expectedThreeTerms = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.1)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.1)))))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.1))
                );

            // Act
            IExpression exprThreeTerms = parser.Parse(ThreeTerms);

            // Assert
            Assert.IsTrue(exprThreeTerms.Equals(expectedThreeTerms));
        }

        [TestMethod]
        public void SimplePower01()
        {
            // Arrange
            Parser parser = new Parser();
            string Square = "x^2.0";
            var expectedSquare = new Power(new BaseX(), new Constant(2.0));

            // Act
            IExpression exprSquare = parser.Parse(Square);

            // Assert
            Assert.IsTrue(exprSquare.Equals(expectedSquare));
        }

        [TestMethod]
        public void SimplePower02()
        {
            // Arrange
            Parser parser = new Parser();
            string InvertedSquare = "x^(-2.0)";
            var expectedInvertedSquare = new Power(new BaseX(), new Constant(-2.0));

            // Act
            IExpression exprInvertedSquare = parser.Parse(InvertedSquare);

            // Assert
            Assert.IsTrue(exprInvertedSquare.Equals(expectedInvertedSquare));
        }

        [TestMethod]
        public void SimplePower03()
        {
            // Arrange
            Parser parser = new Parser();
            string InvertedSquareInvalid = "x^-2.0";
            var expectedInvalid = new InvalidExpression();

            // Act
            IExpression exprInvalid = parser.Parse(InvertedSquareInvalid);

            // Assert
            Assert.IsTrue(exprInvalid.Equals(expectedInvalid));
        }

        [TestMethod]
        public void SimplePower04()
        {
            // Arrange
            Parser parser = new Parser();
            string PowerString = "2.0^x";
            var expectedPower = new Power(new Constant(2.0), new BaseX());

            // Act
            IExpression exprPower = parser.Parse(PowerString);

            // Assert
            Assert.IsTrue(exprPower.Equals(expectedPower));
        }

        [TestMethod]
        public void SimplePower05()
        {
            // Arrange
            Parser parser = new Parser();
            string PowerString = "-(2.0^x)";
            var expectedPower = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new Power(new Constant(2.0), new BaseX())));

            // Act
            IExpression exprPower = parser.Parse(PowerString);

            // Assert
            Assert.IsTrue(exprPower.Equals(expectedPower));
        }

        /// <summary>
        /// Tests the correct parsing of a power tower - basically, start at the back.
        /// </summary>
        [TestMethod]
        public void PowerTower()
        {
            // Arrange
            Parser parser = new Parser();
            string PowerString = "3.0^x^2.0";
            var expectedPower = new Power(new Constant(3.0), new Power(new BaseX(), new Constant(2.0)));

            // Act
            IExpression exprPower = parser.Parse(PowerString);

            // Assert
            Assert.IsTrue(exprPower.Equals(expectedPower));
        }

        /// <summary>
        /// Roundtrip test on the members of <see cref="Helper.TestExpressionBuilder"/>
        /// </summary>
        [TestMethod]
        public void Complex01()
        {
            // Arrange
            Parser parser = new Parser();
            IExpression exprExpected = TestExpressionBuilder.Expression01();
            string printed = ExpressionPrinter.PrintExpression(exprExpected);

            // Act
            IExpression exprParsed = parser.Parse(printed);

            // Assert
            Assert.IsTrue(exprExpected.Equals(exprParsed));
        }

        /// <summary>
        /// Roundtrip test on the members of <see cref="Helper.TestExpressionBuilder"/>
        /// </summary>
        [TestMethod]
        public void Complex02()
        {
            // Arrange
            Parser parser = new Parser();
            IExpression exprExpected = TestExpressionBuilder.Expression02();
            string printed = ExpressionPrinter.PrintExpression(exprExpected);

            // Act
            IExpression exprParsed = parser.Parse(printed);

            // Assert
            Assert.IsTrue(exprExpected.Equals(exprParsed));
        }

        /// <summary>
        /// Roundtrip test on the members of <see cref="Helper.TestExpressionBuilder"/>
        /// </summary>
        [TestMethod]
        public void Complex03()
        {
            // Arrange
            Parser parser = new Parser();
            IExpression exprExpected = TestExpressionBuilder.Expression03();
            string printed = ExpressionPrinter.PrintExpression(exprExpected);

            // Act
            IExpression exprParsed = parser.Parse(printed);

            // Assert
            Assert.IsTrue(exprExpected.Equals(exprParsed));
        }

        /// <summary>
        /// Roundtrip test on the members of <see cref="Helper.TestExpressionBuilder"/>
        /// </summary>
        [TestMethod]
        public void Complex04()
        {
            // Arrange
            Parser parser = new Parser();
            IExpression exprExpected = TestExpressionBuilder.Expression04();
            string printed = ExpressionPrinter.PrintExpression(exprExpected);

            // Act
            IExpression exprParsed = parser.Parse(printed);

            // Assert
            Assert.IsTrue(exprExpected.Equals(exprParsed));
        }

        /// <summary>
        /// Roundtrip test on the members of <see cref="Helper.TestExpressionBuilder"/>
        /// </summary>
        [TestMethod]
        public void Complex05()
        {
            // Arrange
            Parser parser = new Parser();
            IExpression exprExpected = TestExpressionBuilder.Expression05();
            string printed = ExpressionPrinter.PrintExpression(exprExpected);

            // Act
            IExpression exprParsed = parser.Parse(printed);

            // Assert
            Assert.IsTrue(exprExpected.Equals(exprParsed));
        }

        /// <summary>
        /// Roundtrip test on the members of <see cref="Helper.TestExpressionBuilder"/>
        /// </summary>
        [TestMethod]
        public void Complex06()
        {
            // Arrange
            Parser parser = new Parser();
            IExpression exprExpected = TestExpressionBuilder.Expression06();
            string printed = ExpressionPrinter.PrintExpression(exprExpected);

            // Act
            IExpression exprParsed = parser.Parse(printed);

            // Assert
            Assert.IsTrue(exprExpected.Equals(exprParsed));
        }

        /// <summary>
        /// Roundtrip test on the members of <see cref="Helper.TestExpressionBuilder"/>
        /// </summary>
        [TestMethod]
        public void Complex07()
        {
            // Arrange
            Parser parser = new Parser();
            IExpression exprExpected = TestExpressionBuilder.Expression07();
            string printed = ExpressionPrinter.PrintExpression(exprExpected);

            // Act
            IExpression exprParsed = parser.Parse(printed);

            // Assert
            Assert.IsTrue(exprExpected.Equals(exprParsed));
        }

        /// <summary>
        /// Roundtrip test on the members of <see cref="Helper.TestExpressionBuilder"/>
        /// </summary>
        [TestMethod]
        public void Complex08()
        {
            // Arrange
            Parser parser = new Parser();
            IExpression exprExpected = TestExpressionBuilder.Expression08();
            string printed = ExpressionPrinter.PrintExpression(exprExpected);

            // Act
            IExpression exprParsed = parser.Parse(printed);

            // Assert
            Assert.IsTrue(exprExpected.Equals(exprParsed));
        }
    }
}
