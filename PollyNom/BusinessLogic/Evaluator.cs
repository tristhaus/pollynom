using System;
using System.Collections.Generic;

namespace PollyNom.BusinessLogic
{
    public class Evaluator
    {
        public List<Tuple<double, double>> Evaluate()
        {
            Expression expression = this.BuildExpression();
            var list = new List<Tuple<double, double>>(10);
            for(int i = 0; i<100; i++)
            {
                var value = expression.Evaluate(0.1*i);
                if(value.HasValue())
                {
                    list.Add(new Tuple<double, double>(0.1 * i, value.Value()));
                }
            }
            return list;
        }

        private Expression BuildExpression()
        {
            BaseX X = new BaseX();
            Power SecondPower = new Power(X, new Constant(2));
            Power ThirdPower = new Power(X, new Constant(3));
            Subtract Addition = new Subtract(SecondPower, ThirdPower);
            return Addition;
        }
    }
}
