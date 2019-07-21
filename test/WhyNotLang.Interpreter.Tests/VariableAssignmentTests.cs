using System;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Test.Common;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class VariableAssignmentTests
    {
        private ProgramState _programState;
        private Executor _executor;

        public VariableAssignmentTests()
        {
            _programState = new ProgramState();
            _executor = TestHelpers.CreateExecutor(_programState);
        }

        [Fact]
        public void ExecutesVariableAssignementWithNumberExpression()
        {
            _executor.Initialise(@"
                var x := 1
                x := 2
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("x");

            var expected = new ExpressionValue(2, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExecutesVariableDeclarationWithComplexExpression()
        {
            _executor.Initialise(@"
                var abc:=100
                abc := (10 * (4/4) + (2 - 1))");
            
            _executor.ExecuteAll();
            
            var actual = _programState.CurrentScope.GetVariable("abc");

            var expected = new ExpressionValue(11, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ThrowsDuringVariableAssignementWhenUndeclared()
        {
            _executor.Initialise(@"
                x := 2
            ");

            Assert.Throws<ArgumentException>(() => _executor.ExecuteAll());
        }
    }
}