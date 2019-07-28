using System;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Interpreter.StatementExecutors
{
    public class ArrayAssignmentExecutor : IStatementExecutor
    {
        private readonly IStatementIterator _statementIterator;
        private readonly IExpressionEvaluator _expressionEvaluator;
        private readonly IProgramState _programState;

        public ArrayAssignmentExecutor(IStatementIterator statementIterator,
            IExpressionEvaluator expressionEvaluator, IProgramState programState)
        {
            _statementIterator = statementIterator;
            _expressionEvaluator = expressionEvaluator;
            _programState = programState;
        }
        
        public async Task<ExpressionValue> Execute()
        {
            var arrayAssignement = _statementIterator.CurrentStatement as ArrayAssignmentStatement;
            var arrayName = arrayAssignement.ArrayName.Value;
            var arrayIndexValue = await _expressionEvaluator.Eval(arrayAssignement.IndexExpression);
            var arrayItemValue = await _expressionEvaluator.Eval(arrayAssignement.ValExpression);

            if (arrayIndexValue.Type != ExpressionValueTypes.Number)
            {
                throw new ArgumentException("Index needs to be a number");
            }
            
            _programState.AssignArrayItem(arrayName, (int) arrayIndexValue.Value, arrayItemValue);
            
            return ExpressionValue.Empty;
        }
    }
}