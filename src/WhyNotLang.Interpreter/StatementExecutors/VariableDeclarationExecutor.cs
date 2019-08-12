using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class VariableDeclarationExecutor : IStatementExecutor
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IExecutor _mainExecutor;

        public VariableDeclarationExecutor(IExpressionEvaluator expressionEvaluator, IExecutor mainExecutor)
        {
            _expressionEvaluator = expressionEvaluator;
            _mainExecutor = mainExecutor;
        }

        public async Task<ExpressionValue> Execute()
        {
            var variableDeclaration = _mainExecutor.CurrentContext.StatementIterator.CurrentStatement as VariableDeclarationStatement;
            if (variableDeclaration == null)
            {
                throw new WhyNotLangException("Variable declaration expected");
            }

            var variableName = variableDeclaration.VariableName.Value;
            var variableValue = await _expressionEvaluator.Eval(variableDeclaration.Expression);
            _mainExecutor.ProgramState.DeclareVariable(variableName, variableValue, variableDeclaration.IsGlobal);

            return ExpressionValue.Empty;
        }
    }
}