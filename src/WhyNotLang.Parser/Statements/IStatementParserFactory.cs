using WhyNotLang.Parser.Statements.Parsers;

namespace WhyNotLang.Parser.Statements
{
    public interface IStatementParserFactory
    {
        IStatementParser CreateStatementParser(Parser parser);
    }
}