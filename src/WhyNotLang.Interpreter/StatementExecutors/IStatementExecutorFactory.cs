namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementExecutorFactory
    {
        IStatementExecutor CreateStatementExecutor();
    }
}