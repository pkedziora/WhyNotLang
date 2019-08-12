using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class IfStatementTests
    {
        private readonly IProgramState _programState;
        private readonly IExecutor _executor;

        public IfStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = _executor.ProgramState;
        }

        [Fact]
        public async Task ExecutesSimpleTrueIfStatement()
        {
            _executor.Initialise(@"
                if (1 == 1)
                    var x := 2
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(2, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ExecutesTrueIfStatementWithBlock()
        {
            _executor.Initialise(@"
                var z:= 0
                if (!(1 > 2) and 1 <= 2)
                begin
                    var x:=3
                    x:=4
                    z:=x
                end
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("z");

            var expected = new ExpressionValue(4, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("x"));
        }

        [Fact]
        public async Task ExecutesSimpleFalseIfStatement()
        {
            _executor.Initialise(@"
                if (1 != 1)
                    var x := 2
            ");

            await _executor.ExecuteAll();

            Assert.False(_programState.IsVariableDefined("x"));
        }

        [Fact]
        public async Task ExecutesIfElseStatement()
        {
            _executor.Initialise(@"
                if (1 == 2)
                    var x := 2
                else
                    var x:= 3
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(3, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ExecutesIfElseIfStatementMiddleTrue()
        {
            _executor.Initialise(@"
                if (1 == 2)
                    var x := 2
                else if (1 == 1)
                    var x:= 3
                else
                    var x:= 4
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(3, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ExecutesIfElseIfStatementLastTrue()
        {
            _executor.Initialise(@"
                if (1 == 2)
                    var x := 2
                else if (1 > 1)
                    var x:= 3
                else
                    var x:= 4
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(4, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
    }
}