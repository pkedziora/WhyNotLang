using System.Collections.Generic;
using WhyNotLang.Parser.Statements.Parsers;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements
{
    public interface IStatementParserMap
    {
        Dictionary<TokenType, IStatementParser> GetMap(IParser parser);
    }
}