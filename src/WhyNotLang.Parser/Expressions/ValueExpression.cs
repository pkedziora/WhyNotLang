using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Expressions
{
    public class ValueExpression : IExpression
    {
        public Token Token { get; }
        public ExpressionType Type => ExpressionType.Value;

        public ValueExpression(Token token)
        {
            Token = token;
        }

        public override bool Equals(object obj)
        {
            var expression = obj as ValueExpression;
            if (expression == null)
            {
                return false;
            }
            
            return Token.Equals(expression.Token) && Type == expression.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Token.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();

                return hash;
            }
        }
    }
}