using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class BlockStatementExecutor : IStatementExecutor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IProgramState _programState;

        public BlockStatementExecutor(IStatementIterator statementIterator, IProgramState programState)
        {
            _statementIterator = statementIterator;
            _programState = programState;
        }
        
        public ExpressionValue Execute()
        {
            var blockStatement = _statementIterator.CurrentStatement as BlockStatement;

            _programState.AddScope("Block");
            var newExecutor = Executor.CreateExecutor(blockStatement.ChildStatements, _programState);
            var returnValue = newExecutor.ExecuteAll();
            _programState.RemoveScope();
            
            return returnValue;
        }
    }
}