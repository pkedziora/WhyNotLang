using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class BuiltinFunctionsTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public BuiltinFunctionsTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = serviceProvider.GetService<IProgramState>();
        }
        
        [Fact]
        public void ExecutesBuiltinFunctionToNumber()
        {
            _executor.Initialise(@"
                var x:= 100 + ToNumber(""2"")       
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(102, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesBuiltinFunctionToString()
        {
            _executor.Initialise(@"
                var x:= ""1"" + ToString(0)      
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue("10", ExpressionValueTypes.String);

            Assert.Equal(expected, actual);
        }
    }
}