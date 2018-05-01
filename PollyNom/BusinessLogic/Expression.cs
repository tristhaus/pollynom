namespace PollyNom.BusinessLogic
{
    public abstract class Expression
    {
        public abstract int Level { get; }

        public abstract bool IsMonadic { get; }

        public abstract Maybe<double> Evaluate(double input);

        public abstract Maybe<string> Print();
    }
}
