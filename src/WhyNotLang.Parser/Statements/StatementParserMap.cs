using System.Collections.Generic;
using WhyNotLang.Parser.Statements.Parsers;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class StatementParserMap : IStatementParserMap
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;
        private Dictionary<TokenType, IStatementParser> _map;

        public StatementParserMap(ITokenIterator tokenIterator, IExpressionParser expressionParser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
           
        }

        public Dictionary<TokenType, IStatementParser> GetMap(IParser parser)
        {
            if (_map == null)
            {
                _map = new Dictionary<TokenType, IStatementParser>
                {
                    {TokenType.Var, new VariableDeclarationParser(_tokenIterator, _expressionParser)},
                    {TokenType.Identifier, new VariableAssignmentParser(_tokenIterator, _expressionParser)},
                    {TokenType.If, new IfStatementParser(_tokenIterator, _expressionParser, parser)}
                };
            }

            return _map;
        }
    }
}