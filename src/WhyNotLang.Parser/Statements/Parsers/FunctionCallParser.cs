using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements.Parsers
{
    public class FunctionCallParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;

        public FunctionCallParser(ITokenIterator tokenIterator, IExpressionParser expressionParser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
        }

        public IStatement Parse()
        {
            var lineNumber = _tokenIterator.CurrentToken.LineNumber;
            if (_tokenIterator.CurrentToken.Type != TokenType.Identifier && _tokenIterator.PeekToken(1).Type != TokenType.LeftParen)
            {
                throw new WhyNotLangException("function call expected", lineNumber);
            }

            var functionExpression = (FunctionExpression)_expressionParser.ParseNextExpression();

            return new FunctionCallStatement(functionExpression, lineNumber);
        }
    }
}