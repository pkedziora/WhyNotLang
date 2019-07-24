using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Test.Common;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class BuiltinFunctionsTests
    {
        private ProgramState _programState;
        private Executor _executor;

        public BuiltinFunctionsTests()
        {
            _programState = new ProgramState();
            _executor = TestHelpers.CreateExecutor(_programState);
        }
        
        [Fact]
        public void ExecutesBuiltinFunctionToNumber()
        {
            _executor.Initialise(@"
                var x:= 100 + ToNumber(""2"")       
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("x");

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
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue("10", ExpressionValueTypes.String);

            Assert.Equal(expected, actual);
        }
    }
}