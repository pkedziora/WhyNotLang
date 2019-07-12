using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Tests
{
    public class TestHelpers
    {
        private static TokenMap _tokenMap = new TokenMap();
        
        public static BinaryExpression GetBinaryExpression(int a, string op, int b)
        {
            var left = new ValueExpression(new Token(TokenType.Number, a.ToString()));
            var right = new ValueExpression(new Token(TokenType.Number, b.ToString()));
            return new BinaryExpression(left, new Token(_tokenMap.Map[op], op), right);
        }
        
        public static BinaryExpression GetBinaryExpression(IExpression left, string op, int b)
        {
            var right = new ValueExpression(new Token(TokenType.Number, b.ToString()));
            return new BinaryExpression(left, new Token(_tokenMap.Map[op], op), right);
        }
        
        public static BinaryExpression GetBinaryExpression(int a, string op, IExpression right)
        {
            var left = new ValueExpression(new Token(TokenType.Number, a.ToString()));
            return new BinaryExpression(left, new Token(_tokenMap.Map[op], op), right);
        }
    }
}