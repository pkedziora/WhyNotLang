using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
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
            _programState = serviceProvider.GetService<IProgramState>();
        }

        [Fact]
        public void ExecutesFunctionWithoutParamsWithSimpleReturn()
        {
            _executor.Initialise(@"
                function foo()
                begin
                    return 1 
                end
                var x:= foo()         
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesFunctionWithParamsAndReturnStatementWithExpression()
        {
            _executor.Initialise(@"
                function foo(y)
                begin
                    var x:= y + 1
                    return x + 1
                end
                var z:= foo(100)         
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("z");

            var expected = new ExpressionValue(102, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesFunctionWith2ParamsAndReturnStatementWithExpression()
        {
            _executor.Initialise(@"
                function foo(x,y)
                begin
                    return x * y
                end
                var z:= foo(2,3)         
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("z");

            var expected = new ExpressionValue(6, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ReturnStatementStopsExecutionInFunction()
        {
            _executor.Initialise(@"
                function foo()
                begin
                    return 1
                    var y:= 100
                end
                var x:= foo()         
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("y"));
        }
        
        [Fact]
        public void ReturnStatementStopsExecutionOutsideFunction()
        {
            _executor.Initialise(@"
                var x := 100
                return 0
                var y := 2
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(100, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("y"));
        }
    }
}