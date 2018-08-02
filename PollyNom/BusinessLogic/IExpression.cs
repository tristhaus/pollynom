namespace PollyNom.BusinessLogic
{
    /// <summary>
    /// Defines a mathematical expression, i.e. a function of x.
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// The precedence level of the underlying operation.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Indicates whether the expression is monadic.
        /// </summary>
        bool IsMonadic { get; }

        /// <summary>
        /// Evaluates the expression at a given x-coordinate.
        /// </summary>
        /// <param name="input">The x-coordinate at which the expression shall be evaluated.</param>
        /// <returns>A <see cref="IMaybe{double}"/> representing the result of the evaluation.</returns>
        IMaybe<double> Evaluate(double input);

        /// <summary>
        /// Prints the expression as a human-readable and machine-parseable string.
        /// </summary>
        /// <returns>A <see cref="IMaybe{double}"/> representing the printed string.</returns>
        IMaybe<string> Print();
    }
}
