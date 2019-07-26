using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class VariableDeclarationTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public VariableDeclarationTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = serviceProvider.GetService<IProgramState>();
        }

        [Fact]
        public void ExecutesVariableDeclarationWithNumberExpression()
        {
            _executor.Initialise("var x := 1");
            
            _executor.ExecuteNext();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesVariableDeclarationWithComplexExpression()
        {
            _executor.Initialise("var abc := (10 * (4/4) + (2 - 1))");
            
            _executor.ExecuteNext();
            
            var actual = _programState.GetVariable("abc");

            var expected = new ExpressionValue(11, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
    }
}