namespace WhyNotLang.Parser.Statements
{
    public class EofStatement : IStatement
    {
        public StatementType Type => StatementType.EofStatement;
        public int LineNumber => 0;
    }
}