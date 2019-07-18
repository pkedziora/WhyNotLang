using System;
using System.Collections.Generic;
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
                leftExpression = ParseUnaryExpression();
            }

            while (_tokenIterator.CurrentToken.Type != TokenType.Eof && _tokenIterator.CurrentToken.GetPrecedence() > previousPrecedence)
            {
                var currentOperator = _tokenIterator.CurrentToken;
                var currentPrecedence = currentOperator.GetPrecedence();

                _tokenIterator.GetNextToken();

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
                        rightExpression = ParseUnaryExpression();
                    }
                    else
                    {
                        rightExpression = ParseExpression(currentPrecedence);
                    }
                }

                var binaryExpression = new BinaryExpression(leftExpression, currentOperator, rightExpression);
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
            
            _tokenIterator.GetNextToken(); //Set to new token
            return expression;
        }

        public IExpression ParseFunctionExpression()
        {
            var functionNameToken = _tokenIterator.CurrentToken;
            _tokenIterator.GetNextToken(); // Swallow function name
            var parameterExpressions = new List<IExpression>();
            if (_tokenIterator.PeekToken(1).Type != TokenType.RightParen)
            {
                do
                {
                    _tokenIterator.GetNextToken(); // Swallow ( or ,
                    parameterExpressions.Add(ParseExpression(Precedence.None));
                } while (_tokenIterator.CurrentToken.Type == TokenType.Comma);
            }
            else
            {
                _tokenIterator.GetNextToken(); // Swallow (
                parameterExpressions.Add(new EmptyExpression());
            }

            _tokenIterator.GetNextToken(); //Set to new token
            return new FunctionExpression(functionNameToken, parameterExpressions);
        }
        
        public IExpression ParseUnaryExpression()
        {
            var token = _tokenIterator.CurrentToken;
            var nextToken = _tokenIterator.PeekToken(1);
            var isFunctionExpression = token.Type == TokenType.Identifier && nextToken.Type == TokenType.LeftParen;

            if (isFunctionExpression)
            {
                return ParseFunctionExpression();
            }
            
            if (token.Type == TokenType.Number || token.Type == TokenType.Identifier)
            {
                _tokenIterator.GetNextToken(); //Set to new token
                return new ValueExpression(token);
            }

            _tokenIterator.GetNextToken();
            var inner = ParseUnaryExpression();
            return new UnaryExpression(inner, token);
        }
    }
}