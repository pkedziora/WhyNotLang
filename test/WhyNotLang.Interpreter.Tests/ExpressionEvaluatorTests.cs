using WhyNotLang.Interpreter.Evaluators;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser;
using WhyNotLang.Test.Common;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class ExpressionEvaluatorTests
    {
        private ExpressionEvaluator _expressionEvaluator;
        private ExpressionParser _expressionParser;
        private ProgramState _programState;

        public ExpressionEvaluatorTests()
        {
            _expressionParser = TestHelpers.CreateExpressionParser();
            _programState = new ProgramState();
            _expressionEvaluator = new ExpressionEvaluator(_programState);
        }
        
        [Theory]
        [InlineData("1", 1)]
        [InlineData("1 + 2", 3)]
        [InlineData("1 + ---2", -1)]
        [InlineData("1 + 2 + 3", 6)]
        [InlineData("1 + 2 * 3", 7)]
        [InlineData("(1 + 2) * 3", 9)]
        [InlineData("2*(1 + 2) * 3", 18)]
        [InlineData("2*(1 + 2) * -3", -18)]
        public void EvalBinaryExpressionWithNumbers(string strExpression, int expectedResult)
        {
            var input = _expressionParser.ParseExpression(strExpression);

            var actual = _expressionEvaluator.Eval(input);
            var expected = new ExpressionValue(expectedResult, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData("b", 2)]
        [InlineData("1 + a", 2)]
        [InlineData("1 + ---b", -1)]
        [InlineData("a + b + c", 6)]
        [InlineData("1 + 2 * c", 7)]
        [InlineData("(a + b) * 3", 9)]
        [InlineData("2*(a + 2) * c", 18)]
        [InlineData("2*(1 + b) * -c", -18)]
        public void EvalBinaryExpressionWithNumbersAndVariables(string strExpression, int expectedResult)
        {
            _programState.CurrentScope.DeclareVariable("a", new ExpressionValue(1,ExpressionValueTypes.Number));
            _programState.CurrentScope.DeclareVariable("b", new ExpressionValue(2,ExpressionValueTypes.Number));
            _programState.CurrentScope.DeclareVariable("c", new ExpressionValue(3,ExpressionValueTypes.Number));
            
            var input = _expressionParser.ParseExpression(strExpression);

            var actual = _expressionEvaluator.Eval(input);
            var expected = new ExpressionValue(expectedResult, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData("+1", 1)]
        [InlineData("+-1", -1)]
        [InlineData("-1", -1)]
        [InlineData("--1", 1)]
        [InlineData("---1", -1)]
        [InlineData("--+-1", -1)]
        public void EvalUnaryExpressionWithNumbers(string strExpression, int expectedResult)
        {
            var input = _expressionParser.ParseExpression(strExpression);

            var actual = _expressionEvaluator.Eval(input);
            var expected = new ExpressionValue(expectedResult, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData("1 < 2", 1)]
        [InlineData("1 > 2", 0)]
        [InlineData("1 > 2 or 1 <= 2", 1)]
        [InlineData("1 > 2 and 1 <= 2", 0)]
        [InlineData("1 >= 2 or 1 == 1 and 2 >= 1", 1)]
        [InlineData("(1 == 1 or 2 == 3) and 2 > 1", 1)]
        [InlineData("1 == 2", 0)]
        public void EvalConditionalBinaryExpressionWithNumbers(string strExpression, int expectedResult)
        {
            var input = _expressionParser.ParseExpression(strExpression);

            var actual = _expressionEvaluator.Eval(input);
            var expected = new ExpressionValue(expectedResult, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void EvalSingleString()
        {
            var input = _expressionParser.ParseExpression("\"abc\"");

            var actual = _expressionEvaluator.Eval(input);
            var expected = new ExpressionValue("abc", ExpressionValueTypes.String);
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EvalBinaryExpressionWithStrings()
        {
            var input = _expressionParser.ParseExpression("\"abc\" + \"def\"");

            var actual = _expressionEvaluator.Eval(input);
            var expected = new ExpressionValue("abcdef", ExpressionValueTypes.String);
            
            Assert.Equal(expected, actual);
        }
    }
}