using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
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
            _programState = _executor.ProgramState;
        }
        
        [Fact]
        public async Task ExecutesBuiltinFunctionToNumber()
        {
            _executor.Initialise(@"
                var x:= 100 + ToNumber(""2"")       
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(102, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ExecutesBuiltinFunctionToString()
        {
            _executor.Initialise(@"
                var x:= ""1"" + ToString(0)      
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue("10", ExpressionValueTypes.String);

            Assert.Equal(expected, actual);
        }
    }
}