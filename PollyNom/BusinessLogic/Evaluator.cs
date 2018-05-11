using System;
using System.Collections.Generic;
using PollyNom.BusinessLogic.Expressions;

namespace PollyNom.BusinessLogic
{
    public class Evaluator
    {
        string input;
        public Evaluator(string input)
        {
            this.input = input;
        }

        public List<Tuple<double, double>> Evaluate()
        {
            IExpression expression = this.BuildExpression();
            var list = new List<Tuple<double, double>>(100);
            for (int i = 0; i < 100; i++)
            {
                var value = expression.Evaluate(0.1 * i);
                if (value.HasValue())
                {
                    list.Add(new Tuple<double, double>(0.1 * i, value.Value()));
                }
            }
            return list;
        }

        private IExpression BuildExpression()
        {
            BaseX X = new BaseX();
            Power SecondPower = new Power(X, new Constant(2));
            Power ThirdPower = new Power(X, new Constant(3));
            Add Addition = new Add(new Add.AddExpression(Add.AddExpression.Signs.Plus, SecondPower), new Add.AddExpression(Add.AddExpression.Signs.Minus, ThirdPower));
            return Addition;
        }
    }
}
