using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Statements;

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
            var conditionValue = await _expressionEvaluator.Eval(ifStatement.Condition);

            IStatement statementToExecute;
            if ((int) conditionValue.Value != 0)
            {
                statementToExecute = ifStatement.Body;
            }
            else
            {
                statementToExecute = ifStatement.ElseStatement;
            }
            
            
            _mainExecutor.CreateNewContext(new List<IStatement> {statementToExecute});
            var value = await _mainExecutor.ExecuteAll();
            _mainExecutor.LeaveContext();
            
            return value;
        }
    }
}