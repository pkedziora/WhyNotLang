using System;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements.Parsers
{
    public class IfStatementParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;
        private readonly IParser _parser;

        public IfStatementParser(ITokenIterator tokenIterator, IExpressionParser expressionParser, IParser parser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
            _parser = parser;
        }
        
        public IStatement Parse()
        {
            if (_tokenIterator.CurrentToken.Type != TokenType.If)
            {
                throw new WhyNotLangException("if expected");
            }
            
            _tokenIterator.GetNextToken(); // Set current to (
            if (_tokenIterator.CurrentToken.Type != TokenType.LeftParen)
            {
                throw new WhyNotLangException("( expected");
            }
            
            var condition = _expressionParser.ParseNextExpression();
            var body = _parser.ParseNext();
            IStatement elseStatement = null;
            if (_tokenIterator.CurrentToken.Type == TokenType.Else)
            {
                _tokenIterator.GetNextToken(); // Swallow else
                elseStatement = _parser.ParseNext();
            }
            
            return new IfStatement(condition, body, elseStatement);
        }
    }
}