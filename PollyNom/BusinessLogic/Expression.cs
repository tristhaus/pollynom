using System;

namespace PollyNom.BusinessLogic
{
    public abstract class Expression
    {
        public abstract int Level { get; }

        public abstract bool IsMonadic { get; }

        public abstract Maybe<double> Evaluate(double input);

        public abstract Maybe<string> Print();
    }

    public class InvalidExpression : Expression
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
                return -1;
            }
        }

        public override Maybe<double> Evaluate(double input)
        {
            return new None<double>();
        }

        public override Maybe<string> Print()
        {
            return new None<string>();
        }
    }

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

    public class Add : Expression
    {
        private Expression a;
        private Expression b;

        public Add(Expression a, Expression b)
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
            return new Some<double>(aValue.Value() + bValue.Value());
        }

        public override Maybe<string> Print()
        {
            var aValue = this.a.Print();
            var bValue = this.b.Print();
            if (!aValue.HasValue() || !bValue.HasValue())
            {
                return new None<string>();
            }
            return new Some<string>(aValue.Value() + "+" + bValue.Value());
        }
    }

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
            if(a.Level == this.Level-1)
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

    public class Divide : Expression
    {
        private Expression a;
        private Expression b;

        public Divide(Expression a, Expression b)
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
            if (!aValue.HasValue() || !bValue.HasValue() || Math.Abs(bValue.Value()) < 10e-10)
            {
                return new None<Double>();
            }
            return new Some<double>(aValue.Value() / bValue.Value());
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

            return new Some<string>(aDecorated + "/" + bDecorated);
        }
    }

    public class Power : Expression
    {
        private Expression a;
        private Expression b;

        public Power(Expression a, Expression b)
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
                return 3;
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
            return new Some<double>(Math.Pow(aValue.Value(), bValue.Value()));
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
