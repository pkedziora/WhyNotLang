using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public interface IStatementExecutorFactory
    {
        IStatementExecutor CreateStatementExecutor(StatementType statementType, IExecutor mainExecutor);
    }
}