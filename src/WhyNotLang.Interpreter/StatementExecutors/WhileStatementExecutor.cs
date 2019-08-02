using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class WhileStatementExecutor : IStatementExecutor
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IExecutor _mainExecutor;

        public WhileStatementExecutor(IExpressionEvaluator expressionEvaluator, IExecutor mainExecutor)
        {
            _expressionEvaluator = expressionEvaluator;
            _mainExecutor = mainExecutor;
        }
        
        public async Task<ExpressionValue> Execute()
        {
            var whileStatement = _mainExecutor.CurrentContext.StatementIterator.CurrentStatement as WhileStatement;
            _mainExecutor.CreateNewContext(new List<IStatement> { whileStatement.Body});
            ExpressionValue returnValue = ExpressionValue.Empty;
            while (!_mainExecutor.Stopped && (int) (await _expressionEvaluator.Eval(whileStatement.Condition)).Value != 0)
            {
                returnValue = await _mainExecutor.ExecuteAll();
                if (!returnValue.Equals(ExpressionValue.Empty))
                {
                    break;
                }

                _mainExecutor.CurrentContext.ResetPosition();
            }
            _mainExecutor.LeaveContext();
            return returnValue;
        }
    }
}