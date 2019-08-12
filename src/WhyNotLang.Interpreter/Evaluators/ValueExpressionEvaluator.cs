using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Interpreter.Evaluators
{
    public class ValueExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IProgramState _programState;

        public ValueExpressionEvaluator(IProgramState programState)
        {
            _programState = programState;
        }

        public async Task<ExpressionValue> Eval(IExpression expression)
        {
            var valueExpression = expression as ValueExpression;
            if (valueExpression == null || expression.Type != ExpressionType.Value)
            {
                throw new WhyNotLangException("ValueExpression expected");
            }

            var token = valueExpression.Token;
            switch (valueExpression.Token.Type)
            {
                case TokenType.Number:
                    return await Task.FromResult(new ExpressionValue(int.Parse(token.Value), ExpressionValueTypes.Number));
                case TokenType.String:
                    return await Task.FromResult(new ExpressionValue(token.Value, ExpressionValueTypes.String));
                case TokenType.Identifier:
                    return await Task.FromResult(GetVariableValue(token.Value));
            }

            throw new WhyNotLangException("Unsupported token type");
        }

        private ExpressionValue GetVariableValue(string identifier)
        {
            return _programState.GetVariable(identifier);
        }
    }
}