namespace PollyNom.BusinessLogic.Expressions
{
    public class Constant : IExpression
    {
        private double a;

        public Constant(double a)
        {
            this.a = a;
        }

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
            return new Some<double>(a);
        }

        public Maybe<string> Print()
        {
            return new Some<string>($"{a}");
        }
    }
}
