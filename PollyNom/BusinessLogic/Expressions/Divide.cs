using System;

namespace PollyNom.BusinessLogic.Expressions
{
    public class Divide : Multiply
    {
        public Divide(Expression a, Expression b) :
        base( new MultiplyExpression(MultiplyExpression.Signs.Multiply, a), new MultiplyExpression(MultiplyExpression.Signs.Divide, b))
        {
        }
    }
}
