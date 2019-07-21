using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class VariableDeclarationExecutor : IStatementExecutor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramState _programState;

        public VariableDeclarationExecutor(IStatementIterator statementIterator,
            IExpressionEvaluator expressionEvaluator, IProgramState programState)
        {
            _statementIterator = statementIterator;
            _expressionEvaluator = expressionEvaluator;
            _programState = programState;
        }
        
        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}