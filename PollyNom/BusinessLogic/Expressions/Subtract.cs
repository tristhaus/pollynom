using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Subtract : Expression
    {
        private Expression a;
        private Expression b;

        public Subtract(Expression a, Expression b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool IsMonadic
        {
            get
            {
                return false;
            }
        }

        public override int Level
        {
            get
            {
                return 1;
            }
        }

        public override Maybe<double> Evaluate(double input)
        {
            var aValue = this.a.Evaluate(input);
            var bValue = this.b.Evaluate(input);
            if (!aValue.HasValue() || !bValue.HasValue())
            {
                return new None<Double>();
            }
            return new Some<double>(aValue.Value() - bValue.Value());
        }

        public override Maybe<string> Print()
        {
            var aValue = this.a.Print();
            var bValue = this.b.Print();
            if (!aValue.HasValue() || !bValue.HasValue())
            {
                return new None<string>();
            }
            return new Some<string>(aValue.Value() + "-" + bValue.Value());
        }
    }
}
