using System;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Extensions;
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

            return ParseExpression(Precedence.None);
        }
        
        private IExpression ParseExpression(Precedence previousPrecedence)
        {
            var leftExpression = ParseUnaryExpression(_tokenIterator.CurrentToken);
            while (_tokenIterator.PeekToken(1).Type != TokenType.Eof && _tokenIterator.PeekToken(1).GetPrecedence() > previousPrecedence)
            {
                var currentOperator = _tokenIterator.GetNextToken();
                var currentPrecedence = currentOperator.GetPrecedence();

                var nextToken = _tokenIterator.GetNextToken();
                var nextPrecedence = _tokenIterator.PeekToken(1).GetPrecedence();
                
                IExpression rightExpression;
                if (currentPrecedence >= nextPrecedence)
                {
                    rightExpression = ParseUnaryExpression(nextToken);
                }
                else
                {
                    rightExpression = ParseExpression(currentPrecedence);
                }
                
                var binaryExpression = ParseBinaryExpression(leftExpression, currentOperator, rightExpression);
                leftExpression = binaryExpression;
            }

            return leftExpression;
        }

        public IExpression ParseUnaryExpression(Token token)
        {
            if (token.Type == TokenType.Number || token.Type == TokenType.Identifier)
            {
                return new ValueExpression(token);
            }

            var inner = ParseUnaryExpression(_tokenIterator.GetNextToken());
            return new UnaryExpression(inner, token);
        }
        
        public IExpression ParseBinaryExpression(IExpression left, Token operatorToken, IExpression right)
        {
            return new BinaryExpression(left, operatorToken, right);
        }
    }
}