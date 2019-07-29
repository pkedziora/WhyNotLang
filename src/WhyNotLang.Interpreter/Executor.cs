using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Interpreter.StatementExecutors;
using WhyNotLang.Parser;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter
{
    public class Executor : IExecutor
    {
        private readonly IParser _parser;
        public IProgramState ProgramState { get; }
        public IExecutorContext CurrentContext => _executorContexts.Peek();
        private readonly Stack<IExecutorContext> _executorContexts;
        public Executor(IProgramState programState, IParser parser)
        {
            _parser = parser;
            ProgramState = programState;
            _executorContexts = new Stack<IExecutorContext>();
            CreateNewContext(new List<IStatement>());
        }

        public void Initialise(string program)
        {
            _parser.Initialise(program);
            var statements = _parser.ParseAll();
            CurrentContext.StatementIterator.InitStatements(statements);
        }
        
        public async Task<ExpressionValue> ExecuteNext()
        {
            var executor =  CurrentContext.StatementExecutorFactory
                                .CreateStatementExecutor(CurrentContext.StatementIterator.CurrentStatement.Type, 
                                    this);
            var value = await executor.Execute();
            if (!Equals(value, ExpressionValue.Empty))
            {
                return value;
            }
            
            CurrentContext.StatementIterator.GetNextStatement();
            
            return ExpressionValue.Empty;
        }

        public void ResetPosition()
        {
            CurrentContext.StatementIterator.ResetPosition();
        }
        
        public async Task<ExpressionValue> ExecuteAll()
        {
            while (CurrentContext.StatementIterator.CurrentStatement.Type != StatementType.EofStatement)
            {
                var value = await ExecuteNext();
                if (!Equals(value, ExpressionValue.Empty))
                {
                    return value;
                }
            }
            
            return ExpressionValue.Empty;
        }
        
        public void CreateNewContext(List<IStatement> statements)
        {
            var iterator = new StatementIterator(statements);

            var statementExecutionFactory = new StatementExecutorFactory(
                new ExpressionEvaluator(this, new BuiltinFunctionEvaluator(ProgramState)));
            _executorContexts.Push(new ExecutorContext(iterator, ProgramState, statementExecutionFactory));
        }

        public void LeaveContext()
        {
            _executorContexts.Pop();
        }
    }
}