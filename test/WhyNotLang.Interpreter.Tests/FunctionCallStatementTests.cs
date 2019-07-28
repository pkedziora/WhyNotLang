using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class FunctionCallStatementTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public FunctionCallStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = serviceProvider.GetService<IProgramState>();
        }

        [Fact]
        public async Task ExecutesFunctionWithoutParams()
        {
            _executor.Initialise(@"
                function foo()
                begin
                    var x:= 100
                    return x
                end
                var x:= foo()         
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(100, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ExecutesFunctionWithParams()
        {
            _executor.Initialise(@"
                function foo(y)
                begin
                    var x:= y + 1
                    return x
                end
                var z := foo(100)         
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("z");

            var expected = new ExpressionValue(101, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ExecutesFunctionWith2Params()
        {
            _executor.Initialise(@"
                function foo(x,y)
                begin
                    var z:= x * y
                    return z
                end
                var x:= foo(2,3)         
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(6, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task FunctionCanBeUsedWithinExpression()
        {
            _executor.Initialise(@"
                function square(a)
                begin
                    return a * a
                end
                var x:= 1 + square(2) * 3
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(13, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("y"));
        }
    }
}