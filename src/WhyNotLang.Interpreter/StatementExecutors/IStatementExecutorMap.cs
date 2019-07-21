namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementExecutorMap
    {
        IStatementExecutor GetStatementExecutor();
    }
}