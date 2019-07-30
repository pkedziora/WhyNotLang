using System;
using System.Collections.Generic;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Extensions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser
{
    public class ExpressionParser : IExpressionParser
    {
        private readonly ITokenIterator _tokenIterator;
        public ExpressionParser(ITokenIterator tokenIterator)
        {
            _tokenIterator = tokenIterator;
        }

        public IExpression ParseExpression(string expression)
        {
            _tokenIterator.InitTokens(expression);

            return ParseExpression(Precedence.None);
        }
        
        public IExpression ParseNextExpression()
        {
            return ParseExpression(Precedence.None);
        }

        private IExpression ParseExpression(Precedence previousPrecedence)
        {
            IExpression leftExpression;

            if (_tokenIterator.CurrentToken.Type == TokenType.LeftParen)
            {
                leftExpression = ParseParens();
            }
            else
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
                    var nextPrecedence = GetNextPrecedence();
                
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

        private Precedence GetNextPrecedence()
        {
            var isFunctionCallOrArray = _tokenIterator.CurrentToken.Type == TokenType.Identifier && 
                                 (_tokenIterator.PeekToken(1).Type == TokenType.LeftParen || 
                                  _tokenIterator.PeekToken(1).Type == TokenType.LeftBracket);
            
            if (isFunctionCallOrArray)
            {
                // Find closing paren of function call
                var openParens = 1;
                var peekAhead = 2;
                while (openParens > 0)
                {
                    if (_tokenIterator.PeekToken(peekAhead).Type == TokenType.LeftParen || 
                        _tokenIterator.PeekToken(peekAhead).Type == TokenType.LeftBracket)
                    {
                        openParens++;
                    }
                    else if (_tokenIterator.PeekToken(peekAhead).Type == TokenType.RightParen ||
                             _tokenIterator.PeekToken(peekAhead).Type == TokenType.RightBracket)
                    {
                        openParens--;
                    }

                    peekAhead++;
                }

                return _tokenIterator.PeekToken(peekAhead).GetPrecedence(); // Get precedence of operator after the function call
            }
            
            return _tokenIterator.PeekToken(1).GetPrecedence();
        }

        private IExpression ParseParens()
        {
            if (_tokenIterator.CurrentToken.Type != TokenType.LeftParen)
            {
                throw new WhyNotLangException("( expected");
            }

            _tokenIterator.GetNextToken(); // Swallow (

            var expression = ParseExpression(Precedence.None);
            
            _tokenIterator.GetNextToken();
            return expression;
        }

        private IExpression ParseFunctionExpression()
        {
            if (_tokenIterator.CurrentToken.Type != TokenType.Identifier)
            {
                throw new WhyNotLangException("Identifier expected");
            }
            
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

            _tokenIterator.GetNextToken();
            return new FunctionExpression(functionNameToken, parameterExpressions);
        }
        
        private IExpression ParseArrayExpression()
        {
            if (_tokenIterator.CurrentToken.Type != TokenType.Identifier)
            {
                throw new WhyNotLangException("Identifier expected");
            }
            
            var arrayNameToken = _tokenIterator.CurrentToken;
            _tokenIterator.GetNextToken(); // Swallow array name
            _tokenIterator.GetNextToken(); // Swallow [

            var indexExpression = ParseExpression(Precedence.None);

            _tokenIterator.GetNextToken();
            return new ArrayExpression(arrayNameToken, indexExpression);
        }

        private IExpression ParseUnaryExpression()
        {
            var token = _tokenIterator.CurrentToken;
            var nextToken = _tokenIterator.PeekToken(1);
            
            var isFunctionExpression = token.Type == TokenType.Identifier && nextToken.Type == TokenType.LeftParen;
            if (isFunctionExpression)
            {
                return ParseFunctionExpression();
            }
            
            var isArrayExpression = token.Type == TokenType.Identifier && nextToken.Type == TokenType.LeftBracket;
            if (isArrayExpression)
            {
                return ParseArrayExpression();
            }

            if (_tokenIterator.CurrentToken.Type == TokenType.LeftParen)
            {
                return ParseParens();
            }
            
            if (token.Type == TokenType.Number || token.Type == TokenType.Identifier || token.Type == TokenType.String)
            {
                _tokenIterator.GetNextToken();
                return new ValueExpression(token);
            }

            _tokenIterator.GetNextToken();
            var inner = ParseUnaryExpression();
            return new UnaryExpression(inner, token);
        }
    }
}