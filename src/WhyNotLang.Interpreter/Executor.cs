using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Interpreter.StatementExecutors;
using WhyNotLang.Parser;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter
{
    public class Executor : IExecutor
    {
        public IProgramState ProgramState { get; }
        public IExecutorContext CurrentContext => _executorContexts.Peek();
        public bool Stopped { get; private set; }

        private readonly IParser _parser;
        private readonly IStatementExecutorFactory _statementExecutorFactory;
        private readonly Stack<IExecutorContext> _executorContexts;

        public Executor(IProgramState programState, IParser parser, IStatementExecutorFactory statementExecutorFactory)
        {
            _parser = parser;
            ProgramState = programState;
            _statementExecutorFactory = statementExecutorFactory;
            _executorContexts = new Stack<IExecutorContext>();
            CreateNewContext(new List<IStatement>());
        }

        public void Initialise(string program)
        {
            Stopped = false;
            _parser.Initialise(program);
            var statements = _parser.ParseAll();
            CurrentContext.StatementIterator.InitStatements(statements);
        }
        
        public async Task<ExpressionValue> ExecuteNext()
        {
            var executor =  _statementExecutorFactory
                                .CreateOrGetFromCache(CurrentContext.StatementIterator.CurrentStatement.Type, 
                                    this);
            var value = await executor.Execute();
            if (!Equals(value, ExpressionValue.Empty))
            {
                return value;
            }
            
            CurrentContext.StatementIterator.GetNextStatement();
            
            return await Task.FromResult(ExpressionValue.Empty);
        }

        public async Task<ExpressionValue> ExecuteAll()
        {
            while (!Stopped && CurrentContext.StatementIterator.CurrentStatement.Type != StatementType.EofStatement)
            {
                var value = await ExecuteNext();
                if (!Equals(value, ExpressionValue.Empty))
                {
                    return value;
                }
            }
            
            return await Task.FromResult(ExpressionValue.Empty);
        }
        
        public void CreateNewContext(List<IStatement> statements)
        {
            var iterator = new StatementIterator(statements);
            
            _executorContexts.Push(new ExecutorContext(iterator));
        }

        public void LeaveContext()
        {
            _executorContexts.Pop();
        }

        public void Stop()
        {
            Stopped = true;
        }
    }
}