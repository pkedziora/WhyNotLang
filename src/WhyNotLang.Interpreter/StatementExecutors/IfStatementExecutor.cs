using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class IfStatementExecutor : IStatementExecutor
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IExecutor _mainExecutor;

        public IfStatementExecutor(IExpressionEvaluator expressionEvaluator, IExecutor mainExecutor)
        {
            _expressionEvaluator = expressionEvaluator;
            _mainExecutor = mainExecutor;
        }

        public async Task<ExpressionValue> Execute()
        {
            var ifStatement = _mainExecutor.CurrentContext.StatementIterator.CurrentStatement as IfStatement;
            if (ifStatement == null)
            {
                throw new WhyNotLangException("If statement expected");
            }

            var conditionValue = await _expressionEvaluator.Eval(ifStatement.Condition);
            var statementToExecute = (int)conditionValue.Value != 0 ? ifStatement.Body : ifStatement.ElseStatement;

            _mainExecutor.CreateNewContext(new List<IStatement> { statementToExecute });
            var value = await _mainExecutor.ExecuteAll();
            _mainExecutor.LeaveContext();

            return value;
        }
    }
}