using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements.Parsers
{
    public class ReturnStatementParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;

        public ReturnStatementParser(ITokenIterator tokenIterator, IExpressionParser expressionParser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
        }
        
        public IStatement Parse()
        {
            var lineNumber = _tokenIterator.CurrentToken.LineNumber;
            if (_tokenIterator.CurrentToken.Type != TokenType.Return)
            {
                throw new WhyNotLangException("return expected", lineNumber);
            }
            
            _tokenIterator.GetNextToken(); // Swallow return

            IExpression returnExpression = _expressionParser.ParseNextExpression();

            var statement = new ReturnStatement(returnExpression, lineNumber);

            return statement;
        }
    }
}