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
            IExpression leftExpression = ParseParens();
            if (leftExpression == null)
            {
                leftExpression = ParseUnaryExpression(_tokenIterator.CurrentToken);;
            }

            while (_tokenIterator.PeekToken(1).Type != TokenType.Eof && _tokenIterator.PeekToken(1).GetPrecedence() > previousPrecedence)
            {
                var currentOperator = _tokenIterator.GetNextToken();
                var currentPrecedence = currentOperator.GetPrecedence();

                var nextToken = _tokenIterator.GetNextToken();

                IExpression rightExpression;
                if (_tokenIterator.CurrentToken.Type == TokenType.LeftParen)
                {
                    rightExpression = ParseExpression(currentPrecedence);
                }
                else
                {
                    var nextPrecedence = _tokenIterator.PeekToken(1).GetPrecedence();
                
                    if (currentPrecedence >= nextPrecedence)
                    {
                        rightExpression = ParseUnaryExpression(nextToken);
                    }
                    else
                    {
                        rightExpression = ParseExpression(currentPrecedence);
                    }
                }

                var binaryExpression = ParseBinaryExpression(leftExpression, currentOperator, rightExpression);
                leftExpression = binaryExpression;
            }

            return leftExpression;
        }

        public IExpression ParseParens()
        {
            if (_tokenIterator.CurrentToken.Type != TokenType.LeftParen)
            {
                return null;
            }

            _tokenIterator.GetNextToken(); // Current is one after (

            var expression = ParseExpression(Precedence.None);
            
            _tokenIterator.GetNextToken(); // Current is )

            return expression;

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