using WhyNotLang.Parser.Statements.Parsers;

namespace WhyNotLang.Parser.Statements
{
    public interface IStatementParserMap
    {
        IStatementParser GetStatementParser(Parser parser);
    }
}