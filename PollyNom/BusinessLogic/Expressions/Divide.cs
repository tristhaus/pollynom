using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Divide : Multiply
    {
        public Divide(IExpression a, IExpression b) :
        base( new MultiplyExpression(MultiplyExpression.Signs.Multiply, a), new MultiplyExpression(MultiplyExpression.Signs.Divide, b))
        {
        }
    }
}
