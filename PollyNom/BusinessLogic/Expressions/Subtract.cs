namespace PollyNom.BusinessLogic.Expressions
{
    public class Subtract : Add
    {
        public Subtract(Expression a, Expression b) :
        base( new AddExpression(AddExpression.Signs.Plus, a), new AddExpression(AddExpression.Signs.Minus, b) )    
        {
        }
    }
}
