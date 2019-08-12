using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class FunctionCallStatementExecutor : IStatementExecutor
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IExecutor _mainExecutor;

        public FunctionCallStatementExecutor(IExpressionEvaluator expressionEvaluator, IExecutor mainExecutor)
        {
            _expressionEvaluator = expressionEvaluator;
            _mainExecutor = mainExecutor;
        }

        public async Task<ExpressionValue> Execute()
        {
            var callStatement = _mainExecutor.CurrentContext.StatementIterator.CurrentStatement as FunctionCallStatement;
            if (callStatement == null)
            {
                throw new WhyNotLangException("Function call expected");
            }

            await _expressionEvaluator.Eval(callStatement.FunctionExpression);

            return ExpressionValue.Empty;
        }
    }
}