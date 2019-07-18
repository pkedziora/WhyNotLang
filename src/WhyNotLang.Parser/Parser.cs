using System.Collections.Generic;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Parser.Statements.Parsers;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser
{
    public class Parser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IStatementParserMap _statementParserMap;

        public Parser(ITokenIterator tokenIterator, IStatementParserMap statementParserMap)
        {
            _tokenIterator = tokenIterator;
            _statementParserMap = statementParserMap;
        }

        public List<IStatement> Parse()
        {
            var statements = new List<IStatement>();
            while (_tokenIterator.CurrentToken.Type != TokenType.Eof)
            {
                var parser = GetNextStatementParser();
                var statement = parser.Parse();
                statements.Add(statement);
            }

            return statements;
        }

        private IStatementParser GetNextStatementParser()
        {
            return _statementParserMap.Map[_tokenIterator.CurrentToken.Type];
        }
    }
}