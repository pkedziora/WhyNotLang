using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class WhileStatementTests
    {
        private readonly IProgramState _programState;
        private readonly IExecutor _executor;

        public WhileStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = _executor.ProgramState;
        }

        [Fact]
        public async void ExecutesCountingWhileStatement()
        {
            _executor.Initialise(@"
                var x := 0
                while (x < 10)
                    x := x + 1
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(10, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ExecutesWhileStatementWithBlock()
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

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("pow");

            var expected = new ExpressionValue(1024, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void VariableInWhileStatementBlockHidesOuterScope()
        {
            _executor.Initialise(@"
                var x := 0
                var y := 1
                while (x < 10)
                begin
                    var y := 2
                    x := x + 1
                end
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("y");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
    }
}