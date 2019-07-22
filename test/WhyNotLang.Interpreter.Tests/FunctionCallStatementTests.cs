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
        
        [Fact]
        public void ExecutesFunctionWithParams()
        {
            _executor.Initialise(@"
                function foo(y)
                begin
                    var x:= y + 1
                end
                foo(100)         
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue(101, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesFunctionWith2Params()
        {
            _executor.Initialise(@"
                function foo(x,y)
                begin
                    var z:= x * y
                end
                foo(2,3)         
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("z");

            var expected = new ExpressionValue(6, ExpressionValueTypes.Number);

            Assert.Equal(expected, actual);
        }
    }
}