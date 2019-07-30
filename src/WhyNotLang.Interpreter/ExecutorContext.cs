using WhyNotLang.Interpreter.StatementExecutors;

namespace WhyNotLang.Interpreter
{
    public class ExecutorContext : IExecutorContext
    {
        public IStatementIterator StatementIterator { get; }

        public ExecutorContext(IStatementIterator statementIterator)
        {
            StatementIterator = statementIterator;
        }

        public void ResetPosition()
        {
            StatementIterator.ResetPosition();
        }
    }
}