using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Multiply : Expression
    {
        private Expression a;
        private Expression b;

        public Multiply(Expression a, Expression b)
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
                return 2;
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
            return new Some<double>(aValue.Value() * bValue.Value());
        }

        public override Maybe<string> Print()
        {
            var aValue = this.a.Print();
            var bValue = this.b.Print();
            if (!aValue.HasValue() || !bValue.HasValue())
            {
                return new None<string>();
            }

            var aDecorated = aValue.Value();
            var bDecorated = bValue.Value();
            if (a.Level == this.Level - 1)
            {
                aDecorated = "(" + aDecorated + ")";
            }
            if (b.Level == this.Level - 1)
            {
                bDecorated = "(" + bDecorated + ")";
            }

            return new Some<string>(aDecorated + "*" + bDecorated);
        }
    }
}
