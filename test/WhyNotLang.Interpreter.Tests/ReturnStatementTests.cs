using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class ReturnStatementTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public ReturnStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = _executor.ProgramState;
        }

        [Fact]
        public async Task ExecutesFunctionWithoutParamsWithSimpleReturn()
        {
            _executor.Initialise(@"
                func foo()
                begin
                    return 1 
                end
                var x:= foo()         
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ExecutesFunctionWithParamsAndReturnStatementWithExpression()
        {
            _executor.Initialise(@"
                func foo(y)
                begin
                    var x:= y + 1
                    return x + 1
                end
                var z:= foo(100)         
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("z");

            var expected = new ExpressionValue(102, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ExecutesFunctionWith2ParamsAndReturnStatementWithExpression()
        {
            _executor.Initialise(@"
                func foo(x,y)
                begin
                    return x * y
                end
                var z:= foo(2,3)         
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("z");

            var expected = new ExpressionValue(6, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ReturnStatementStopsExecutionInFunction()
        {
            _executor.Initialise(@"
                func foo()
                begin
                    return 1
                    var y:= 100
                end
                var x:= foo()         
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("y"));
        }
        
        [Fact]
        public async Task ReturnStatementStopsExecutionOutsideFunction()
        {
            _executor.Initialise(@"
                var x := 100
                return 0
                var y := 2
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(100, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("y"));
        }

        [Fact]
        public async Task ReturnsFromIfStatement()
        {
            _executor.Initialise(@"
                func foo()
                begin
                    if (1 < 2)
                        return 1
                    var y:= 100
                    return 2
                end
                var x:= foo()         
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("y"));
        }

        [Fact]
        public async Task ReturnsFromIfStatementWithBlock()
        {
            _executor.Initialise(@"
                func foo()
                begin
                    if (1 < 2)
                    begin
                        return 1
                    end
                    var y:= 100
                    return 2
                end
                var x:= foo()         
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("y"));
        }

        [Fact]
        public async Task ReturnsFromWhileStatement()
        {
            _executor.Initialise(@"
                func foo()
                begin
                    while (1 < 2)
                        return 1
                    var y:= 100
                    return 2
                end
                var x:= foo()         
            ");

            await _executor.ExecuteAll();

            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("y"));
        }
    }
}