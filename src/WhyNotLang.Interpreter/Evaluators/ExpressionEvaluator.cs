using System;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IExecutor _mainExecutor;

        public ExpressionEvaluator(IExecutor mainExecutor)
        {
            _mainExecutor = mainExecutor;
        }
        
        public async Task<ExpressionValue> Eval(IExpression expression)
        {
            var evaluator = GetExpressionEvaluator(expression);
            return await evaluator.Eval(expression);
        }

        private IExpressionEvaluator GetExpressionEvaluator(IExpression expression)
        {
            switch (expression.Type)
            {
                case ExpressionType.Value:
                    return new ValueExpressionEvaluator(_mainExecutor.ProgramState);
                case ExpressionType.Unary:
                    return new UnaryExpressionEvaluator(this);
                case ExpressionType.Binary:
                    return new BinaryExpressionEvaluator(this);
                case ExpressionType.Function:
                    return new FunctionExpressionEvaluator(this, _mainExecutor);
                case ExpressionType.Array:
                    return new ArrayExpressionEvaluator(this, _mainExecutor.ProgramState);
            }
            
            throw new WhyNotLangException($"Parser not found for expression {expression.Type}");
        }
    }
}