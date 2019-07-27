using System;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class ArrayExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IExpressionEvaluator _mainEvaluator;
        private readonly IProgramState _programState;

        public ArrayExpressionEvaluator(IExpressionEvaluator mainEvaluator, IProgramState programState)
        {
            _mainEvaluator = mainEvaluator;
            _programState = programState;
        }
        
        public ExpressionValue Eval(IExpression expression)
        {
            if (expression.Type != ExpressionType.Array)
            {
                throw new ArgumentException("ArrayExpression expected");
            }
            
            var arrayExpression = expression as ArrayExpression;

            var indexExpression = _mainEvaluator.Eval(arrayExpression.IndexExpression);

            if (indexExpression.Type != ExpressionValueTypes.Number)
            {
                throw new ArgumentException("Index must be a number");
            }
            
            return _programState.GetArrayItem(arrayExpression.Name.Value, (int) indexExpression.Value);
        }
    }
}