using WhyNotLang.Parser.Statements.Parsers;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementExecutorMap
    {
        IStatementExecutor GetStatementExecutor();
    }
}