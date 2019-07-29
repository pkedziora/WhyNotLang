using WhyNotLang.Interpreter.State;
using WhyNotLang.Interpreter.StatementExecutors;

namespace WhyNotLang.Interpreter
{
    public class ExecutorContext : IExecutorContext
    {
        private IProgramState _programState;
        public IStatementIterator StatementIterator { get; }
        public IStatementExecutorFactory StatementExecutorFactory { get; }

        public ExecutorContext(IStatementIterator statementIterator, IProgramState programState, IStatementExecutorFactory statementExecutorFactory)
        {
            _programState = programState;
            StatementIterator = statementIterator;
            StatementExecutorFactory = statementExecutorFactory;
        }
    }
}