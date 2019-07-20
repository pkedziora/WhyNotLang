namespace WhyNotLang.Parser.Statements.Parsers
{
    public class BlockStatementParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;
        private readonly IParser _parser;

        public BlockStatementParser(ITokenIterator tokenIterator, IExpressionParser expressionParser, IParser parser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
            _parser = parser;
        }
        
        public IStatement Parse()
        {
            throw new System.NotImplementedException();
        }
    }
}