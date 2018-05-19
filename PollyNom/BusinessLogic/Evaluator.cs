namespace PollyNom.BusinessLogic
{
    public class Evaluator
    {
        private string input;
        private Parser parser;
        private IExpression expression;

        public Evaluator(string input)
        {
            this.input = input;
            this.parser = new Parser();
            expression = parser.Parse(this.input);
        }

        public Evaluator(IExpression expression)
        {
            this.input = string.Empty;
            this.expression = expression;
        }

        public Maybe<double> Evaluate(double x)
        {
            return expression.Evaluate(x);
        }
    }
}
