using System;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

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
        
        public async Task<ExpressionValue> Eval(IExpression expression)
        {
            if (expression.Type != ExpressionType.Array)
            {
                throw new WhyNotLangException("ArrayExpression expected");
            }
            
            var arrayExpression = expression as ArrayExpression;

            var indexExpression = await _mainEvaluator.Eval(arrayExpression.IndexExpression);

            if (indexExpression.Type != ExpressionValueTypes.Number)
            {
                throw new WhyNotLangException("Index must be a number");
            }
            
            return _programState.GetArrayItem(arrayExpression.Name.Value, (int) indexExpression.Value);
        }
    }
}