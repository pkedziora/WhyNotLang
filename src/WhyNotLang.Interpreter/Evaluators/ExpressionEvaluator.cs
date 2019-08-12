using System.Collections.Generic;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        private readonly Dictionary<ExpressionType, IExpressionEvaluator> _evaluatorCache;
        private readonly IExecutor _mainExecutor;

        public ExpressionEvaluator(IExecutor mainExecutor)
        {
            _mainExecutor = mainExecutor;
            _evaluatorCache = new Dictionary<ExpressionType, IExpressionEvaluator>();
        }

        public async Task<ExpressionValue> Eval(IExpression expression)
        {
            var evaluator = CreateOrGetFromCache(expression.Type);
            return await evaluator.Eval(expression);
        }

        public IExpressionEvaluator CreateOrGetFromCache(ExpressionType expressionType)
        {
            if (!_evaluatorCache.TryGetValue(expressionType, out var expressionEvaluator))
            {
                expressionEvaluator = CreateExpressionEvaluator(expressionType);
                _evaluatorCache[expressionType] = expressionEvaluator;
            }

            return expressionEvaluator;
        }

        private IExpressionEvaluator CreateExpressionEvaluator(ExpressionType expressionType)
        {
            switch (expressionType)
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

            throw new WhyNotLangException($"Parser not found for expression {expressionType}");
        }
    }
}