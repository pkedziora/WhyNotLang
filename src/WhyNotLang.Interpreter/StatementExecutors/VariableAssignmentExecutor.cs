using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class VariableAssignmentExecutor : IStatementExecutor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramState _programState;

        public VariableAssignmentExecutor(IStatementIterator statementIterator,
            IExpressionEvaluator expressionEvaluator, IProgramState programState)
        {
            _statementIterator = statementIterator;
            _expressionEvaluator = expressionEvaluator;
            _programState = programState;
        }
        
        public async Task<ExpressionValue> Execute()
        {
            var variableAssignment = _statementIterator.CurrentStatement as VariableAssignmentStatement;
            var variableName = variableAssignment.VariableName.Value;
            var variableValue = await _expressionEvaluator.Eval(variableAssignment.Expression);
            _programState.AssignVariable(variableName, variableValue);
            
            return ExpressionValue.Empty;
        }
    }
}