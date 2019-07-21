namespace WhyNotLang.Parser.Statements
{
    public class EofStatement : IStatement
    {
        public StatementType Type => StatementType.EofStatement;
    }
}