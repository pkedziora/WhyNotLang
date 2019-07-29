using System;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IExecutor _mainExecutor;
        private readonly IBuiltinFunctionEvaluator _builtinEvaluator;

        public ExpressionEvaluator(IExecutor mainExecutor, IBuiltinFunctionEvaluator builtinEvaluator)
        {
            _mainExecutor = mainExecutor;
            _builtinEvaluator = builtinEvaluator;
        }
        
        public async Task<ExpressionValue> Eval(IExpression expression)
        {
            var evaluator = GetExpressionEvaluator(expression);
            return await evaluator.Eval(expression);
        }
        
        public IExpressionEvaluator GetExpressionEvaluator(IExpression expression)
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
                    return new FunctionExpressionEvaluator(this, _builtinEvaluator, _mainExecutor);
                case ExpressionType.Array:
                    return new ArrayExpressionEvaluator(this, _mainExecutor.ProgramState);
            }
            
            throw new ArgumentException($"Parser not found for expression {expression.Type}");
        }
    }
}