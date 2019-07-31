using System;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements.Parsers
{
    public class WhileStatementParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;
        private readonly IParser _parser;

        public WhileStatementParser(ITokenIterator tokenIterator, IExpressionParser expressionParser, IParser parser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
            _parser = parser;
        }
        
        public IStatement Parse()
        {
            var firstLineNumber = _tokenIterator.CurrentToken.LineNumber;
            if (_tokenIterator.CurrentToken.Type != TokenType.While)
            {
                throw new WhyNotLangException("while expected", firstLineNumber);
            }
            
            _tokenIterator.GetNextToken(); // Set current to (
            if (_tokenIterator.CurrentToken.Type != TokenType.LeftParen)
            {
                throw new WhyNotLangException("( expected", _tokenIterator.CurrentToken.LineNumber);
            }
            
            var condition = _expressionParser.ParseNextExpression();
            var body = _parser.ParseNext();

            return new WhileStatement(condition, body, firstLineNumber);
        }
    }
}