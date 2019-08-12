using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class VariableAssignmentExecutor : IStatementExecutor
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IExecutor _mainExecutor;

        public VariableAssignmentExecutor(IExpressionEvaluator expressionEvaluator, IExecutor mainExecutor)
        {
            _expressionEvaluator = expressionEvaluator;
            _mainExecutor = mainExecutor;
        }

        public async Task<ExpressionValue> Execute()
        {
            var variableAssignment = _mainExecutor.CurrentContext.StatementIterator.CurrentStatement as VariableAssignmentStatement;
            var variableName = variableAssignment.VariableName.Value;
            var variableValue = await _expressionEvaluator.Eval(variableAssignment.Expression);
            _mainExecutor.ProgramState.AssignVariable(variableName, variableValue);

            return ExpressionValue.Empty;
        }
    }
}