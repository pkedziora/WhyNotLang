using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class FunctionCallStatementExecutor : IStatementExecutor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramState _programState;

        public FunctionCallStatementExecutor(IStatementIterator statementIterator, IExpressionEvaluator expressionEvaluator, IProgramState programState)
        {
            _statementIterator = statementIterator;
            _expressionEvaluator = expressionEvaluator;
            _programState = programState;
        }
        
        public async Task<ExpressionValue> Execute()
        {
            var callStatement = _statementIterator.CurrentStatement as FunctionCallStatement;
            await _expressionEvaluator.Eval(callStatement.FunctionExpression);
            
            return ExpressionValue.Empty;
        }
    }
}