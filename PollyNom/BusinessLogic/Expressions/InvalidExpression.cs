using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollyNom.BusinessLogic.Expressions
{
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
}
