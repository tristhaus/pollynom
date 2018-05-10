using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollyNom.BusinessLogic;
using PollyNom.BusinessLogic.Expressions;

namespace PollyNomTest
{
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

        [TestMethod]
        public void SimpleAdditionTests()
        {
            // Arrange
            Parser parser = new Parser();

            string TwoAdd = "+2.0+3.0";
            var expectedTwoAdd = new Add(new Constant(2.0), new Constant(3.0));

            string TwoSubtract = "-2.0-3.0";
            var expectedTwoSubtract = new Add(new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(2.0)), new Add.AddExpression(Add.AddExpression.Signs.Minus, new Constant(3.0)));

            string TwoAddBracketed = "(2.0)+(3.0)";
            var expectedTwoAddBracketed = new Add(new Constant(2.0), new Constant(3.0));

            string ThreeAdd = "2.0+3.0-x";
            var expectedThreeAdd = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(2.0)), new Add.AddExpression(Add.AddExpression.Signs.Plus, new Constant(3.0)), new Add.AddExpression(Add.AddExpression.Signs.Minus, new BaseX()));

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
            IExpression exprTwoAdd = parser.Parse(TwoAdd);
            IExpression exprTwoSubtract = parser.Parse(TwoSubtract);
            IExpression exprTwoAddBracketed = parser.Parse(TwoAddBracketed);
            IExpression exprThreeAdd = parser.Parse(ThreeAdd);
            IExpression exprThreeAddWithBrackets = parser.Parse(ThreeAddWithBrackets);

            // Assert
            Assert.IsTrue(exprTwoAdd.Equals(expectedTwoAdd));
            Assert.IsTrue(expectedTwoSubtract.Equals(exprTwoSubtract));
            Assert.IsTrue(exprTwoAddBracketed.Equals(expectedTwoAddBracketed));
            Assert.IsTrue(exprThreeAdd.Equals(expectedThreeAdd));
            Assert.IsTrue(exprThreeAddWithBrackets.Equals(expectedThreeAddWithBrackets));
        }
    }
}
