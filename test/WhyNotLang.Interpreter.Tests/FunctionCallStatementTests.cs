using System.Collections.Generic;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Test.Common;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class FunctionCallStatementTests
    {
        private ProgramState _programState;
        private Executor _executor;

        public FunctionCallStatementTests()
        {
            _programState = new ProgramState();
            _executor = TestHelpers.CreateExecutor(_programState);
        }

        [Fact]
        public void ExecutesFunctionWithoutParams()
        {
            _executor.Initialise(@"
                function foo()
                begin
                    var x:= 100
                end
                foo()         
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue(100, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
    }
}