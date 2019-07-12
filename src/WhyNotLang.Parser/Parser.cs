using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser
{
    public class Parser
    {
        private readonly ITokenIterator _tokenIterator;

        public Parser(ITokenIterator tokenIterator)
        {
            _tokenIterator = tokenIterator;
        }

        public IExpression ParseExpression(string expression)
        {
            _tokenIterator.InitTokens(expression);
            
            var leftExpression = ParseValueExpression(_tokenIterator.CurrentToken);

            while (_tokenIterator.GetNextToken().Type != TokenType.Eof)
            {
                var operatorToken = _tokenIterator.CurrentToken;
                var rightExpression = ParseValueExpression(_tokenIterator.GetNextToken());
                var binaryExpression = ParseBinaryExpression(leftExpression, operatorToken, rightExpression);
                leftExpression = binaryExpression;
            }

            return leftExpression;
        }

        public IExpression ParseValueExpression(Token token)
        {
            return new ValueExpression(token);
        }
        
        public IExpression ParseBinaryExpression(IExpression left, Token operatorToken, IExpression right)
        {
            return new BinaryExpression(left, operatorToken, right);
        }
    }
}