using System;

namespace PollyNom.BusinessLogic
{
    public abstract class Expression
    {
        public abstract Maybe<double> Evaluate(double input);
    }

    public class BaseX : Expression
    {
        public override Maybe<double> Evaluate(double input)
        {
            return new Some<double>(input);
        }
    }

    public class Constant : Expression
    {
        private double a;

        public Constant(double a)
        {
            this.a = a;
        }

        public override Maybe<double> Evaluate(double input)
        {
            return new Some<double>(a);
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
    }
}
