using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            Expression expression = this.BuildExpression();
            var list = new List<Tuple<double, double>>(100);
            if (ValidateInput())
            {
                for (int i = 0; i < 100; i++)
                {
                    var value = expression.Evaluate(0.1 * i);
                    if (value.HasValue())
                    {
                        list.Add(new Tuple<double, double>(0.1 * i, value.Value()));
                    }
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

        private Expression Parse()
        {
            if(!ValidateInput())
            {
                return new InvalidExpression();
            }
            Expression expression = new Constant(1);

            return expression;
        }

        private bool ValidateInput()
        {
            // check unsupported characters 
            {
                Regex regex = new Regex("^[-0-9+/*^()xX]+$");
                if(!regex.IsMatch(this.input)) {
                    return false;
                }
            }

            // check balanced parentheses
            {
                int count = 0;
                foreach (char c in this.input)
                {
                    if(c == '(')
                    {
                        count++;
                    }
                    else if(c == ')')
                    {
                        count--;
                    }
                    if(count < 0)
                    {
                        return false;
                    }
                }
                if(count != 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
