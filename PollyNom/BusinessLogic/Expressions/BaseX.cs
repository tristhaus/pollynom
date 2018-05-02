namespace PollyNom.BusinessLogic.Expressions
{
    public class BaseX : IExpression
    {
        public bool IsMonadic
        {
            get
            {
                return true;
            }
        }

        public int Level
        {
            get
            {
                return 0;
            }
        }

        public Maybe<double> Evaluate(double input)
        {
            return new Some<double>(input);
        }

        public Maybe<string> Print()
        {
            return new Some<string>("x");
        }
    }
}
