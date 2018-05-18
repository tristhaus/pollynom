using System;
using System.Collections.Generic;
using PollyNom.BusinessLogic.Expressions;

namespace PollyNom.BusinessLogic
{
    public class Evaluator
    {
        private string input;
        private Parser parser;

        public Evaluator(string input)
        {
            this.input = input;
            this.parser = new Parser();
        }

        public List<Tuple<double, double>> Evaluate()
        {
            IExpression expression = parser.Parse(this.input);

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
    }
}
