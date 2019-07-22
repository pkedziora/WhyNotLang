using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.State;
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

        public void Initialise(string program)
        {
            _statementIterator.InitStatements(program);
        }
        
        public void ExecuteNext()
        {
            var executor = _statementExecutorMap.GetStatementExecutor();
            executor.Execute();
            _statementIterator.GetNextStatement();
        }

        public void ResetPosition()
        {
            _statementIterator.ResetPosition();
        }
        
        public void ExecuteAll()
        {
            while (_statementIterator.CurrentStatement.Type != StatementType.EofStatement)
            {
                ExecuteNext();
            }
        }
        
        public static Executor CreateExecutor(List<IStatement> statements, IProgramState programState)
        {
            var iterator = new StatementIterator(statements);
            return new Executor(iterator, new StatementExecutorMap(iterator, new ExpressionEvaluator(programState), programState));
        }
    }
}