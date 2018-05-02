using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollyNom.BusinessLogic.Expressions
{
    public class InvalidExpression : IExpression
    {
        public bool IsMonadic
        {
            get
            {
                return true;
            }
        }

        public int Level
        {
            get
            {
                return -1;
            }
        }

        public Maybe<double> Evaluate(double input)
        {
            return new None<double>();
        }

        public Maybe<string> Print()
        {
            return new None<string>();
        }
    }
}
