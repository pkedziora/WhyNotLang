using System.Linq;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Tests
{
    public class TestHelpers
    {
        private static readonly Tokenizer.Tokenizer _tokenizer = CreateTokenizer();

        public static BinaryExpression GetBinaryExpression(int a, string op, int b)
        {
            var left = new ValueExpression(new Token(TokenType.Number, a.ToString()));
            var right = new ValueExpression(new Token(TokenType.Number, b.ToString()));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpression(IExpression left, string op, int b)
        {
            var right = new ValueExpression(new Token(TokenType.Number, b.ToString()));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpression(int a, string op, IExpression right)
        {
            var left = new ValueExpression(new Token(TokenType.Number, a.ToString()));
            return new BinaryExpression(left, GetToken(op), right);
        }
        
        public static BinaryExpression GetBinaryExpression(IExpression left, string op, IExpression right)
        {
            return new BinaryExpression(left, GetToken(op), right);
        }

        public static UnaryExpression GetUnaryExpression(string op, int number)
        {
            return new UnaryExpression(new ValueExpression(TestHelpers.GetToken(number.ToString())), TestHelpers.GetToken(op));
        }

        public static Token GetToken(string token)
        {
            return _tokenizer.GetTokens(token).FirstOrDefault();
        }

        private static Tokenizer.Tokenizer CreateTokenizer()
        {
            return new Tokenizer.Tokenizer(new TokenReader(), new TokenMap());
        }
    }
}