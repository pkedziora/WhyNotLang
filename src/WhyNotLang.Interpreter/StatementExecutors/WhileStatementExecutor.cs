using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class WhileStatementExecutor : IStatementExecutor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramState _programState;

        public WhileStatementExecutor(IStatementIterator statementIterator, IExpressionEvaluator expressionEvaluator, IProgramState programState)
        {
            _statementIterator = statementIterator;
            _expressionEvaluator = expressionEvaluator;
            _programState = programState;
        }
        
        public async Task<ExpressionValue> Execute()
        {
            var whileStatement = _statementIterator.CurrentStatement as WhileStatement;
            var newExecutor = Executor.CreateExecutor(new List<IStatement> { whileStatement.Body}, _programState);
            while ((int) (await _expressionEvaluator.Eval(whileStatement.Condition)).Value != 0)
            {
                await newExecutor.ExecuteAll();
                newExecutor.ResetPosition();
                await Task.Delay(1);
            }
            
            return ExpressionValue.Empty;
        }
    }
}