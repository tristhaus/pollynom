namespace PollyNom.BusinessLogic
{
    public class Evaluator
    {
        /// <summary>
        /// Original string input, if any.
        /// </summary>
        private string inputString;

        /// <summary>
        /// An instance of the parser, if needed.
        /// </summary>
        private Parser parser;

        /// <summary>
        /// The expression resulting from direct input or from parsing.
        /// </summary>
        private IExpression inputExpression;

        /// <summary>
        /// Creates a new instance of the <see cref="Evaluator"/> class,
        /// accepting a string to be parsed, yielding an <see cref="IExpression"/>
        /// which is to be evaluated.
        /// </summary>
        /// <param name="input">A parseable string representing an expression.</param>
        public Evaluator(string input)
        {
            this.inputString = input;
            this.parser = new Parser();
            inputExpression = parser.Parse(this.inputString);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Evaluator"/> class,
        /// accepting an <see cref="IExpression"/> which is to be evaluated.
        /// </summary>
        /// <param name="expression">An expression.</param>
        public Evaluator(IExpression expression)
        {
            this.inputString = string.Empty;
            this.inputExpression = expression;
        }

        /// <summary>
        /// Evaluates the contained expression at a given x-coordinate <paramref name="x"/>.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <returns>A <see cref="Maybe{double}"/>, which has a value if the expressuion is valid at <paramref name="x"/>.</returns>
        public Maybe<double> Evaluate(double x)
        {
            return inputExpression.Evaluate(x);
        }
    }
}
