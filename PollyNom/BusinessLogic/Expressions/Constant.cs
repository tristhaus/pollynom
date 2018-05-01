namespace PollyNom.BusinessLogic.Expressions
{
    public class Constant : Expression
    {
        private double a;

        public Constant(double a)
        {
            this.a = a;
        }

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
            return new Some<double>(a);
        }

        public override Maybe<string> Print()
        {
            return new Some<string>($"{a}");
        }
    }
}
