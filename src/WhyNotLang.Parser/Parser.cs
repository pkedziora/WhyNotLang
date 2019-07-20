using System.Collections.Generic;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Parser.Statements.Parsers;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser
{
    public class Parser : IParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IStatementParserMap _statementParserMap;

        public Parser(ITokenIterator tokenIterator, IStatementParserMap statementParserMap)
        {
            _tokenIterator = tokenIterator;
            _statementParserMap = statementParserMap;
        }

        public List<IStatement> ParseAll()
        {
            var statements = new List<IStatement>();
            while (_tokenIterator.CurrentToken.Type != TokenType.Eof)
            {
                var statement = ParseNext();
                statements.Add(statement);
            }

            return statements;
        }

        public void Initialise(string program)
        {
            _tokenIterator.InitTokens(program);
        }
        
        public IStatement ParseNext()
        {
            var parser = GetNextStatementParser();
            var statement = parser.Parse();

            return statement;
        }

        private IStatementParser GetNextStatementParser()
        {
            return _statementParserMap.Map[_tokenIterator.CurrentToken.Type];
        }
    }
}