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
                throw new ArgumentException("if expected");
            }
            
            _tokenIterator.GetNextToken(); // Set current to (
            if (_tokenIterator.CurrentToken.Type != TokenType.LeftParen)
            {
                throw new ArgumentException("( expected");
            }
            
            var testExpression = _expressionParser.ParseNextExpression();
            var body = _parser.ParseNext();
            
            return new IfStatement(testExpression, body);
        }
    }
}