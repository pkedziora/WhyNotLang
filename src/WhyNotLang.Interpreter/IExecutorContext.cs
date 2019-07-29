using WhyNotLang.Interpreter.StatementExecutors;

namespace WhyNotLang.Interpreter
{
    public interface IExecutorContext
    {
        IStatementIterator StatementIterator { get; }
        IStatementExecutorFactory StatementExecutorFactory { get; }
    }
}