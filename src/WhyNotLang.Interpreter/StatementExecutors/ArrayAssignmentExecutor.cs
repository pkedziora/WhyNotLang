using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class ArrayAssignmentExecutor : IStatementExecutor
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IExecutor _mainExecutor;

        public ArrayAssignmentExecutor(IExpressionEvaluator expressionEvaluator, IExecutor mainExecutor)
        {
            _expressionEvaluator = expressionEvaluator;
            _mainExecutor = mainExecutor;
        }

        public async Task<ExpressionValue> Execute()
        {
            var arrayAssignment = _mainExecutor.CurrentContext.StatementIterator.CurrentStatement as ArrayAssignmentStatement;
            if (arrayAssignment == null)
            {
                throw new WhyNotLangException("Array assignment expected");
            }

            var arrayName = arrayAssignment.ArrayName.Value;
            var arrayIndexValue = await _expressionEvaluator.Eval(arrayAssignment.IndexExpression);
            var arrayItemValue = await _expressionEvaluator.Eval(arrayAssignment.ValExpression);

            if (arrayIndexValue.Type != ExpressionValueTypes.Number)
            {
                throw new WhyNotLangException("Index needs to be a number", arrayAssignment.ArrayName.LineNumber);
            }

            _mainExecutor.ProgramState.AssignArrayItem(arrayName, (int)arrayIndexValue.Value, arrayItemValue);

            return ExpressionValue.Empty;
        }
    }
}