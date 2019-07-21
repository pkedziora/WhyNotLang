using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Test.Common;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class VariableDeclarationTests
    {
        private ProgramState _programState;
        private Executor _executor;

        public VariableDeclarationTests()
        {
            _programState = new ProgramState();
            _executor = TestHelpers.CreateExecutor(_programState);
        }

        [Fact]
        public void ExecutesVariableDeclarationWithNumberExpression()
        {
            _executor.Initialise("var x := 1");
            
            _executor.ExecuteNext();
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesVariableDeclarationWithComplexExpression()
        {
            _executor.Initialise("var abc := (10 * (4/4) + (2 - 1))");
            
            _executor.ExecuteNext();
            
            var actual = _programState.CurrentScope.GetVariable("abc");

            var expected = new ExpressionValue(11, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
    }
}