using System;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Test.Common;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class IfStatementTests
    {
        private ProgramState _programState;
        private Executor _executor;

        public IfStatementTests()
        {
            _programState = new ProgramState();
            _executor = TestHelpers.CreateExecutor(_programState);
        }

        [Fact]
        public void ExecutesSimpleTrueIfStatement()
        {
            _executor.Initialise(@"
                if (1 == 1)
                    var x := 2
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue(2, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesTrueIfStatementWithBlock()
        {
            _executor.Initialise(@"
                if (!(1 > 2) and 1 <= 2)
                begin
                    var x:=3
                    x:=4
                end
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue(4, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesSimpleFalseIfStatement()
        {
            _executor.Initialise(@"
                if (1 != 1)
                    var x := 2
            ");
            
            _executor.ExecuteAll();

            Assert.False(_programState.CurrentScope.IsVariableDefined("x"));
        }
        
        [Fact]
        public void ExecutesIfElseStatement()
        {
            _executor.Initialise(@"
                if (1 == 2)
                    var x := 2
                else
                    var x:= 3
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue(3, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesIfElseIfStatementMiddleTrue()
        {
            _executor.Initialise(@"
                if (1 == 2)
                    var x := 2
                else if (1 == 1)
                    var x:= 3
                else
                    var x:= 4
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue(3, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesIfElseIfStatementLastTrue()
        {
            _executor.Initialise(@"
                if (1 == 2)
                    var x := 2
                else if (1 > 1)
                    var x:= 3
                else
                    var x:= 4
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue(4, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
    }
}