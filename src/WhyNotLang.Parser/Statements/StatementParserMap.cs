using System.Collections.Generic;
using WhyNotLang.Parser.Statements.Parsers;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public class StatementParserMap : IStatementParserMap
    {
        public Dictionary<TokenType, IStatementParser> Map { get; }

        public StatementParserMap(IExpressionParser expressionParser)
        {
            Map = new Dictionary<TokenType, IStatementParser>
            {
                {TokenType.Var, new VariableDeclarationParser(expressionParser)},
                {TokenType.Identifier, new VariableAssignmentParser(expressionParser)}
            };
        }
    }
}