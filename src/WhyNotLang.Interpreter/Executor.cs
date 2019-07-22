using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
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
        
        public ExpressionValue ExecuteNext()
        {
            var executor = _statementExecutorMap.GetStatementExecutor();
            var value = executor.Execute();
            if (!Equals(value, ExpressionValue.Empty))
            {
                return value;
            }
            
            _statementIterator.GetNextStatement();
            
            return ExpressionValue.Empty;
        }

        public void ResetPosition()
        {
            _statementIterator.ResetPosition();
        }
        
        public ExpressionValue ExecuteAll()
        {
            while (_statementIterator.CurrentStatement.Type != StatementType.EofStatement)
            {
                var value = ExecuteNext();
                if (!Equals(value, ExpressionValue.Empty))
                {
                    return value;
                }
            }
            
            return ExpressionValue.Empty;
        }
        
        public static Executor CreateExecutor(List<IStatement> statements, IProgramState programState)
        {
            var iterator = new StatementIterator(statements);
            return new Executor(iterator, new StatementExecutorMap(iterator, new ExpressionEvaluator(programState), programState));
        }
    }
}