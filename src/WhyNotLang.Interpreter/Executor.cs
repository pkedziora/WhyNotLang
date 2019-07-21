using WhyNotLang.Interpreter.StatementExecutors;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter
{
    public class Executor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly StatementExecutorMap _statementExecutorMap;

        public Executor(IStatementIterator statementIterator, StatementExecutorMap statementExecutorMap)
        {
            _statementIterator = statementIterator;
            _statementExecutorMap = statementExecutorMap;
        }
        
        public void ExecuteNext()
        {
            var executor = _statementExecutorMap.GetStatementExecutor();
            executor.Execute();
            _statementIterator.GetNextStatement();
        }
        
        
        public void ExecuteAll()
        {
            while (_statementIterator.CurrentStatement.Type != StatementType.EofStatement)
            {
                ExecuteNext();
            }
        }
    }
}