using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Expressions;
using PollyNom.BusinessLogic.Expressions.SingleArgumentFunctions;
using PollyNomTest.Helper;

namespace PollyNomTest
{
    /// <summary>
    /// Collects tests related to the parsing of human-readable strings to expressions.
    /// </summary>
    [TestClass]
    public class ParserTest
    {
        /// <summary>
        /// Tests simple expressions.
        /// </summary>
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
        /// Tests whether single addition terms evaluate to Constant, regardless of sign.
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

        /// <summary>
        /// Tests whether a simple expression parses to an <see cref="Add"/> instance.
        /// </summary>
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

        /// <summary>
        /// Tests the correct parsing of simple additive terms.
        /// </summary>
        [TestMethod]
        public void SimpleAddition01()
        {
            // Arrange
            Parser parser = new Parser();
            string twoAdd = "+2.0+3.0";
            var expectedTwoAdd = new Add(new Constant(2.0), new Constant(3.0));

            // Act
            IExpression exprTwoAdd = parser.Parse(twoAdd);

            // Assert
            Assert.IsTrue(exprTwoAdd.Equals(expectedTwoAdd));
        }

        /// <summary>
        /// Tests the correct parsing of simple additive terms involving negative signs.
        /// </summary>
        [TestMethod]
        public void SimpleAddition02()
        {
            // Arrange
            Parser parser = new Parser();
            string twoSubtract = "-2.0-3.0";
            var expectedTwoSubtract = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(-2.0)), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(3.0)));

            // Act
            IExpression exprTwoSubtract = parser.Parse(twoSubtract);

            // Assert
            Assert.IsTrue(expectedTwoSubtract.Equals(exprTwoSubtract));
        }

        /// <summary>
        /// Tests the correct parsing of simple additive terms involving parentheses.
        /// </summary>
        [TestMethod]
        public void SimpleAddition03()
        {
            // Arrange
            Parser parser = new Parser();
            string twoAddBracketed = "(2.0)+(3.0)";
            var expectedTwoAddBracketed = new Add(new Constant(2.0), new Constant(3.0));

            // Act
            IExpression exprTwoAddBracketed = parser.Parse(twoAddBracketed);

            // Assert
            Assert.IsTrue(exprTwoAddBracketed.Equals(expectedTwoAddBracketed));
        }

        /// <summary>
        /// Tests the correct parsing of simple additive terms involving alternating signs.
        /// </summary>
        [TestMethod]
        public void SimpleAddition04()
        {
            // Arrange
            Parser parser = new Parser();
            string threeAdd = "2.0+3.0-x";
            var expectedThreeAdd = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(2.0)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.0)), new Add.AddExpression(Add.AddExpression.Signs.Minus, new BaseX()));

            // Act
            IExpression exprThreeAdd = parser.Parse(threeAdd);

            // Assert
            Assert.IsTrue(exprThreeAdd.Equals(expectedThreeAdd));
        }

        /// <summary>
        /// Tests the correct parsing of simple additive terms involving alternating signs and parentheses.
        /// </summary>
        [TestMethod]
        public void SimpleAddition05()
        {
            // Arrange
            Parser parser = new Parser();
            string threeAddWithBrackets = "2.0-(3.0+x)+1.0";
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
            IExpression exprThreeAddWithBrackets = parser.Parse(threeAddWithBrackets);

            // Assert
            Assert.IsTrue(exprThreeAddWithBrackets.Equals(expectedThreeAddWithBrackets));
        }

        /// <summary>
        /// Tests the correct parsing of simple multiplicative terms.
        /// </summary>
        [TestMethod]
        public void SimpleMultiplication01()
        {
            // Arrange
            Parser parser = new Parser();
            string twoMultiply = "2.0*3.0";
            var expectedTwoMultiply = new Multiply(new Constant(2.0), new Constant(3.0));

            // Act
            IExpression exprTwoMultiply = parser.Parse(twoMultiply);

            // Assert
            Assert.IsTrue(exprTwoMultiply.Equals(expectedTwoMultiply));
        }

        /// <summary>
        /// Tests the correct parsing of simple multiplicative terms involving division.
        /// </summary>
        [TestMethod]
        public void SimpleMultiplication02()
        {
            // Arrange
            Parser parser = new Parser();
            string twoDivide = "2.0/3.0";
            var expectedTwoDivide = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new Constant(3.0)));

            // Act
            IExpression exprTwoDivide = parser.Parse(twoDivide);

            // Assert
            Assert.IsTrue(expectedTwoDivide.Equals(exprTwoDivide));
        }

        /// <summary>
        /// Tests the correct parsing of simple multiplicative terms involving parentheses.
        /// </summary>
        [TestMethod]
        public void SimpleMultiplication03()
        {
            // Arrange
            Parser parser = new Parser();
            string twoMultiplyBracketed = "(2.0)*(3.0)";
            var expectedTwoMultiplyBracketed = new Multiply(new Constant(2.0), new Constant(3.0));

            // Act
            IExpression exprTwoMultiplyBracketed = parser.Parse(twoMultiplyBracketed);

            // Assert
            Assert.IsTrue(exprTwoMultiplyBracketed.Equals(expectedTwoMultiplyBracketed));
        }

        /// <summary>
        /// Tests the correct parsing of simple multiplicative terms involving alternating "signs".
        /// </summary>
        [TestMethod]
        public void SimpleMultiplication04()
        {
            // Arrange
            Parser parser = new Parser();
            string threeMultiply = "2.0*3.0/x";
            var expectedThreeMultiply = new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(3.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Divide, new BaseX()));

            // Act
            IExpression exprThreeMultiply = parser.Parse(threeMultiply);

            // Assert
            Assert.IsTrue(exprThreeMultiply.Equals(expectedThreeMultiply));
        }

        /// <summary>
        /// Tests the correct parsing of simple multiplicative terms involving alternating "signs" and parentheses.
        /// </summary>
        [TestMethod]
        public void SimpleMultiplication05()
        {
            // Arrange
            Parser parser = new Parser();
            string threeMultiplyWithBrackets = "2.0/(3.0*x)*1.0";
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
            IExpression exprThreeMultiplyWithBrackets = parser.Parse(threeMultiplyWithBrackets);

            // Assert
            Assert.IsTrue(exprThreeMultiplyWithBrackets.Equals(expectedThreeMultiplyWithBrackets));
        }

        /// <summary>
        /// Tests the correct parsing of mixed addition and multiplication.
        /// </summary>
        [TestMethod]
        public void AddMultiplyMix01()
        {
            // Arrange
            Parser parser = new Parser();
            string threeTerms = "2.0*x+1.0";
            var expectedThreeTerms = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0))
                );

            // Act
            IExpression exprThreeTerms = parser.Parse(threeTerms);

            // Assert
            Assert.IsTrue(exprThreeTerms.Equals(expectedThreeTerms));
        }

        /// <summary>
        /// Tests the correct parsing of mixed addition and multiplication involving a leading sign.
        /// </summary>
        [TestMethod]
        public void AddMultiplyMix02()
        {
            // Arrange
            Parser parser = new Parser();
            string threeTerms = "-2.0*x+1.0";
            var expectedThreeTerms = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX()))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0))
                );

            // Act
            IExpression exprThreeTerms = parser.Parse(threeTerms);

            // Assert
            Assert.IsTrue(exprThreeTerms.Equals(expectedThreeTerms));
        }

        /// <summary>
        /// Tests the correct parsing of mixed addition and multiplication a leading sign and parentheses.
        /// </summary>
        [TestMethod]
        public void AddMultiplyMix03()
        {
            // Arrange
            Parser parser = new Parser();
            string threeTerms = "-2.1*(x+3.1)+1.1";
            var expectedThreeTerms = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.1)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.1)))))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.1))
                );

            // Act
            IExpression exprThreeTerms = parser.Parse(threeTerms);

            // Assert
            Assert.IsTrue(exprThreeTerms.Equals(expectedThreeTerms));
        }

        /// <summary>
        /// Tests the correct parsing of mixed addition and multiplication a contained sign and parentheses.
        /// </summary>
        [TestMethod]
        public void AddMultiplyMix04()
        {
            // Arrange
            Parser parser = new Parser();
            string threeTerms = "(x+3.1)*-2.1+1.1";
            var expectedThreeTerms = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Multiply(
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(-2.1)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.1)))))
                    ),
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.1))
                );

            // Act
            IExpression exprThreeTerms = parser.Parse(threeTerms);

            // Assert
            Assert.IsTrue(exprThreeTerms.Equals(expectedThreeTerms));
        }

        /// <summary>
        /// Tests the correct parsing of a simple power.
        /// </summary>
        [TestMethod]
        public void SimplePower01()
        {
            // Arrange
            Parser parser = new Parser();
            string square = "x^2.0";
            var expectedSquare = new Power(new BaseX(), new Constant(2.0));

            // Act
            IExpression exprSquare = parser.Parse(square);

            // Assert
            Assert.IsTrue(exprSquare.Equals(expectedSquare));
        }

        /// <summary>
        /// Tests the correct parsing of a simple power involving a negative sign.
        /// </summary>
        [TestMethod]
        public void SimplePower02()
        {
            // Arrange
            Parser parser = new Parser();
            string invertedSquare = "x^(-2.0)";
            var expectedInvertedSquare = new Power(new BaseX(), new Constant(-2.0));

            // Act
            IExpression exprInvertedSquare = parser.Parse(invertedSquare);

            // Assert
            Assert.IsTrue(exprInvertedSquare.Equals(expectedInvertedSquare));
        }

        /// <summary>
        /// Tests the correctly failing parsing of a simple power involving a negative sign.
        /// </summary>
        [TestMethod]
        public void SimplePower03()
        {
            // Arrange
            Parser parser = new Parser();
            string invertedSquareInvalid = "x^-2.0";
            var expectedInvalid = new InvalidExpression();

            // Act
            IExpression exprInvalid = parser.Parse(invertedSquareInvalid);

            // Assert
            Assert.IsTrue(exprInvalid.Equals(expectedInvalid));
        }

        /// <summary>
        /// Tests the correct parsing of a simple power.
        /// </summary>
        [TestMethod]
        public void SimplePower04()
        {
            // Arrange
            Parser parser = new Parser();
            string powerString = "2.0^x";
            var expectedPower = new Power(new Constant(2.0), new BaseX());

            // Act
            IExpression exprPower = parser.Parse(powerString);

            // Assert
            Assert.IsTrue(exprPower.Equals(expectedPower));
        }

        /// <summary>
        /// Tests the correct parsing of a simple power involving a sign.
        /// </summary>
        [TestMethod]
        public void SimplePower05()
        {
            // Arrange
            Parser parser = new Parser();
            string powerString = "-(2.0^x)";
            var expectedPower = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new Power(new Constant(2.0), new BaseX())));

            // Act
            IExpression exprPower = parser.Parse(powerString);

            // Assert
            Assert.IsTrue(exprPower.Equals(expectedPower));
        }

        /// <summary>
        /// Tests the correct parsing of a complex power.
        /// </summary>
        [TestMethod]
        public void LessSimplePower()
        {
            // Arrange
            Parser parser = new Parser();
            string powerString = "-((2.0*x)^(x+1.0))";
            var expectedPower = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus,
                new Power(
                new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX())),
                new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0)))
                )));

            // Act
            IExpression exprPower = parser.Parse(powerString);

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
            string powerString = "3.0^x^2.0";
            var expectedPower = new Power(new Constant(3.0), new Power(new BaseX(), new Constant(2.0)));

            // Act
            IExpression exprPower = parser.Parse(powerString);

            // Assert
            Assert.IsTrue(exprPower.Equals(expectedPower));
        }

        /// <summary>
        /// Tests that a non-recognized function yields an <see cref="InvalidExpression"/>.
        /// </summary>
        [TestMethod]
        public void NonRecognizedFunction()
        {
            // Arrange
            Parser parser = new Parser();
            string stringFunction = "expln(x)"; // try to pass the smell test for invalid characters
            var expectedInvalidExpression = new InvalidExpression();

            // Act
            IExpression expr = parser.Parse(stringFunction);

            // Assert
            Assert.IsTrue(expr.Equals(expectedInvalidExpression));
        }

        /// <summary>
        /// Tests the parsing of exponential and natural logarithm.
        /// </summary>
        [TestMethod]
        public void SimpleExponentialAndNaturalLogarithm()
        {
            // Arrange
            Parser parser = new Parser();

            string stringExponential = "exp(2.0)";
            var expectedExponential = new Exponential(new Constant(2.0));

            string stringLogarithm = "ln(x)";
            var expectedLogarithm = new NaturalLogarithm(new BaseX());

            // Act
            IExpression exprExponential = parser.Parse(stringExponential);
            IExpression exprLogarithm = parser.Parse(stringLogarithm);

            // Assert
            Assert.IsTrue(expectedExponential.Equals(exprExponential));
            Assert.IsTrue(expectedLogarithm.Equals(exprLogarithm));
        }

        /// <summary>
        /// Tests a complicated expression inside an exponential.
        /// </summary>
        [TestMethod]
        public void ComplicatedExponential1()
        {
            // Arrange
            Parser parser = new Parser();
            string stringExponential = "exp(-((2.0*x)^(x+1.0)))";
            var argument = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus,
                new Power(
                new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX())),
                new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0)))
                )));
            var expectedExponential = new Exponential(argument);

            // Act
            IExpression exprExponential = parser.Parse(stringExponential);

            // Assert
            Assert.IsTrue(exprExponential.Equals(expectedExponential));
        }

        /// <summary>
        /// Tests a complicated expression inside an exponential and some decorators.
        /// </summary>
        [TestMethod]
        public void ComplicatedExponential2()
        {
            // Arrange
            Parser parser = new Parser();
            string stringExponential = "4.0+2.3*exp(-((2.0*x)^(x+1.0)))";
            var argument = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus,
                new Power(
                new Multiply(new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.0)), new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new BaseX())),
                new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new BaseX()), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(1.0)))
                )));
            var exponential = new Exponential(argument);
            var expectedExpression = new Add(
                new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(4.0)),
                new Add.AddExpression(Add.AddExpression.Signs.Plus,
                new Multiply(
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, new Constant(2.3)),
                    new Multiply.MultiplyExpression(Multiply.MultiplyExpression.Signs.Multiply, exponential)
                )));

            // Act
            IExpression exprExponential = parser.Parse(stringExponential);

            // Assert
            Assert.IsTrue(exprExponential.Equals(expectedExpression));
        }

        /// <summary>
        /// Tests the parsing of trigonometric functions.
        /// </summary>
        [TestMethod]
        public void SimpleTrigonometricFunctions()
        {
            // Arrange
            Parser parser = new Parser();

            string stringSine = "sin(2.0)";
            var expectedSine = new Sine(new Constant(2.0));

            string stringCosine = "cos(32.0)";
            var expectedCosine = new Cosine(new Constant(32.0));

            string stringTangent = "tan(x)";
            var expectedTangent = new Tangent(new BaseX());

            // Act
            IExpression exprSine = parser.Parse(stringSine);
            IExpression exprCosine = parser.Parse(stringCosine);
            IExpression exprTangent = parser.Parse(stringTangent);

            // Assert
            Assert.IsTrue(expectedSine.Equals(exprSine));
            Assert.IsTrue(expectedCosine.Equals(exprCosine));
            Assert.IsTrue(expectedTangent.Equals(exprTangent));
        }

        /// <summary>
        /// Tests the parsing of the absolute value function.
        /// </summary>
        [TestMethod]
        public void SimpleAbsoluteValue()
        {
            // Arrange
            Parser parser = new Parser();

            string stringAbsoluteValue = "abs(2.0)";
            var expectedAbsoluteValue = new AbsoluteValue(new Constant(2.0));

            // Act
            IExpression exprExponential = parser.Parse(stringAbsoluteValue);

            // Assert
            Assert.IsTrue(expectedAbsoluteValue.Equals(exprExponential));
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

        /// <summary>
        /// Tests the correct result of the parseability evaluation.
        /// </summary>
        [TestMethod]
        public void SimpleParseabilityTests()
        {
            // Arrange
            Parser parser = new Parser();
            string x = "X";
            string xx = "xx";
            string y = "Y";
            string number = " 030.500 ";
            string invalidNumber = "030.50.0";
            string bracedX = "(X)";
            string doubleBracedX = "((X))";

            // Act
            bool resultX = parser.IsParseable(x);
            bool resultXX = parser.IsParseable(xx);
            bool resultInvalid = parser.IsParseable(y);
            bool resultNumber = parser.IsParseable(number);
            bool resultInvalidNumber = parser.IsParseable(invalidNumber);
            bool resultBracedX = parser.IsParseable(bracedX);
            bool resultDoubleBracedX = parser.IsParseable(doubleBracedX);

            // Assert
            Assert.IsTrue(resultX);
            Assert.IsFalse(resultXX);
            Assert.IsFalse(resultInvalid);
            Assert.IsTrue(resultNumber);
            Assert.IsFalse(resultInvalidNumber);
            Assert.IsTrue(resultBracedX);
            Assert.IsTrue(resultDoubleBracedX);
        }

        /// <summary>
        /// Tests that the evaluation of parseability does not throw an exception
        /// and yields the correct result for a specific case.
        /// </summary>
        [TestMethod]
        public void ParseabilityShouldNotThrowExceptions()
        {
            // Arrange
            Parser parser = new Parser();
            string xMinus = "X-";

            // Act
            bool resultXMinus = parser.IsParseable(xMinus);

            // Assert
            Assert.IsFalse(resultXMinus);
        }
    }
}
