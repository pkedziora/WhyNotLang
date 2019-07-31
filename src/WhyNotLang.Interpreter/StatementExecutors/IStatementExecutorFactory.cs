using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementExecutorFactory
    {
        IStatementExecutor CreateOrGetFromCache(IStatement statement, IExecutor mainExecutor);
    }
}