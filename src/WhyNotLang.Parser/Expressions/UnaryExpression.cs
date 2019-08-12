using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Expressions
{
    public class UnaryExpression : IExpression
    {
        public IExpression Inner { get; }
        public Token Operator { get; }

        public ExpressionType Type => ExpressionType.Unary;

        public UnaryExpression(IExpression inner, Token operatorToken)
        {
            Inner = inner;
            Operator = operatorToken;
        }

        public override bool Equals(object obj)
        {
            var expression = obj as UnaryExpression;
            if (expression == null)
            {
                return false;
            }

            return Inner.Equals(expression.Inner) &&
                   Operator.Equals(expression.Operator) &&
                   Type == expression.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Inner.GetHashCode();
                hash = hash * 23 + Operator.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();

                return hash;
            }
        }
    }
}