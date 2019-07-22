using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

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
        
        public ExpressionValue Execute()
        {
            var variableDeclaration = _statementIterator.CurrentStatement as VariableDeclarationStatement;
            var variableName = variableDeclaration.VariableName.Value;
            var variableValue = _expressionEvaluator.Eval(variableDeclaration.Expression);
            _programState.CurrentScope.DeclareVariable(variableName, variableValue);
            
            return ExpressionValue.Empty;
        }
    }
}