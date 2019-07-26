using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class WhileStatementTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public WhileStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = serviceProvider.GetService<IProgramState>();
        }

        [Fact]
        public void ExecutesCountingWhileStatement()
        {
            _executor.Initialise(@"
                var x := 0
                while (x < 10)
                    x := x + 1
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(10, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesWhileStatementWithBlock()
        {
            _executor.Initialise(@"
                var x := 0
                var pow := 1
                while (x < 10)
                begin
                    x := x + 1
                    pow := pow * 2
                end
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("pow");

            var expected = new ExpressionValue(1024, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
    }
}