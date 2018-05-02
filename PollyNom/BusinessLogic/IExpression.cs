namespace PollyNom.BusinessLogic
{
    public interface IExpression
    {
        int Level { get; }

        bool IsMonadic { get; }

        Maybe<double> Evaluate(double input);

        Maybe<string> Print();
    }
}
