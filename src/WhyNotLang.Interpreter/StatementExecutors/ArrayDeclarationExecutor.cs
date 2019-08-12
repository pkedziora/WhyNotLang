using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class ArrayDeclarationExecutor : IStatementExecutor
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IExecutor _mainExecutor;

        public ArrayDeclarationExecutor(IExpressionEvaluator expressionEvaluator, IExecutor mainExecutor)
        {
            _expressionEvaluator = expressionEvaluator;
            _mainExecutor = mainExecutor;
        }

        public async Task<ExpressionValue> Execute()
        {
            var arrayDeclaration = _mainExecutor.CurrentContext.StatementIterator.CurrentStatement as ArrayDeclarationStatement;
            var arrayName = arrayDeclaration.ArrayName.Value;
            var arraySize = await _expressionEvaluator.Eval(arrayDeclaration.IndexExpression);

            _mainExecutor.ProgramState.DeclareArray(arrayName, (int)arraySize.Value, arrayDeclaration.IsGlobal);

            return ExpressionValue.Empty;
        }
    }
}