using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.ExpressionValues;
using WhyNotLang.Test.Common;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class ExpressionEvaluatorTests
    {
        private ExpressionEvaluator _expressionEvaluator;

        public ExpressionEvaluatorTests()
        {
            _expressionEvaluator = new ExpressionEvaluator();
        }

        [Fact]
        public void EvalSingleNumber()
        {
            var input = TestHelpers.GetValueExpression(1);

            var actual = _expressionEvaluator.Eval(input);
            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void EvalSingleString()
        {
            var input = TestHelpers.GetValueExpressionAsString("abc");

            var actual = _expressionEvaluator.Eval(input);
            var expected = new ExpressionValue("abc", ExpressionValueTypes.String);
            
            Assert.Equal(expected, actual);
        }
    }
}