using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementExecutorFactory
    {
        IStatementExecutor CreateOrGetFromCache(StatementType statementType, IExecutor mainExecutor);
    }
}