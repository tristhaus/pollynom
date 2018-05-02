using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Power : IExpression
    {
        private IExpression a;
        private IExpression b;

        public Power(IExpression a, IExpression b)
        {
            this.a = a;
            this.b = b;
        }

        public bool IsMonadic
        {
            get
            {
                return false;
            }
        }

        public int Level
        {
            get
            {
                return 3;
            }
        }

        public Maybe<double> Evaluate(double input)
        {
            var aValue = this.a.Evaluate(input);
            var bValue = this.b.Evaluate(input);
            if (!aValue.HasValue() || !bValue.HasValue())
            {
                return new None<Double>();
            }
            return new Some<double>(Math.Pow(aValue.Value(), bValue.Value()));
        }

        public Maybe<string> Print()
        {
            var aValue = this.a.Print();
            var bValue = this.b.Print();
            if (!aValue.HasValue() || !bValue.HasValue())
            {
                return new None<string>();
            }

            var aDecorated = aValue.Value();
            var bDecorated = bValue.Value();
            if (!a.IsMonadic)
            {
                aDecorated = "(" + aDecorated + ")";
            }
            if (!b.IsMonadic)
            {
                bDecorated = "(" + bDecorated + ")";
            }

            return new Some<string>(aDecorated + "^" + bDecorated);
        }
    }
}
