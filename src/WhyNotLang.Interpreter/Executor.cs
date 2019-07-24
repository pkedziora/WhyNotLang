using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Interpreter.StatementExecutors;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter
{
    public class Executor : IExecutor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IStatementExecutorFactory _statementExecutorFactory;

        public Executor(IStatementIterator statementIterator, IStatementExecutorFactory statementExecutorFactory)
        {
            _statementIterator = statementIterator;
            _statementExecutorFactory = statementExecutorFactory;
        }

        public void Initialise(string program)
        {
            _statementIterator.InitStatements(program);
        }
        
        public ExpressionValue ExecuteNext()
        {
            var executor = _statementExecutorFactory.CreateStatementExecutor();
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
        
        public static IExecutor CreateExecutor(List<IStatement> statements, IProgramState programState)
        {
            var iterator = new StatementIterator(statements);
            return new Executor(iterator, new StatementExecutorFactory(iterator, new ExpressionEvaluator(programState, new BuiltinFunctionEvaluator(programState)), programState));
        }
    }
}