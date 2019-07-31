namespace WhyNotLang.Parser.Statements
{
    public interface IStatement
    {
        StatementType Type { get; }
        int LineNumber { get; }
    }
}