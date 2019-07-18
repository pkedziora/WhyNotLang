using System.Collections.Generic;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Parser
{
    public class StatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;

        public StatementParser(ITokenIterator tokenIterator, IExpressionParser expressionParser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
        }

        public List<IStatement> Parse()
        {
            
        }
    }
}