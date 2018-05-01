namespace PollyNom.BusinessLogic.Expressions
{
    public class BaseX : Expression
    {
        public override bool IsMonadic
        {
            get
            {
                return true;
            }
        }

        public override int Level
        {
            get
            {
                return 0;
            }
        }

        public override Maybe<double> Evaluate(double input)
        {
            return new Some<double>(input);
        }

        public override Maybe<string> Print()
        {
            return new Some<string>("x");
        }
    }
}
