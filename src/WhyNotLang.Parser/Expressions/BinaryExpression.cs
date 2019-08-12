using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Expressions
{
    public class BinaryExpression : IExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }
        public Token Operator { get; }
        public ExpressionType Type => ExpressionType.Binary;

        public BinaryExpression(IExpression left, Token operatorToken, IExpression right)
        {
            Left = left;
            Operator = operatorToken;
            Right = right;
        }

        public override bool Equals(object obj)
        {
            var expression = obj as BinaryExpression;
            if (expression == null)
            {
                return false;
            }

            return Left.Equals(expression.Left) &&
                   Right.Equals(expression.Right) &&
                   Operator.Equals(expression.Operator) &&
                   Type == expression.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Left.GetHashCode();
                hash = hash * 23 + Right.GetHashCode();
                hash = hash * 23 + Operator.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();

                return hash;
            }
        }
    }
}