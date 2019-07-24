using System;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Test.Common;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class IfStatementTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public IfStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = serviceProvider.GetService<IProgramState>();
        }

        [Fact]
        public void ExecutesSimpleTrueIfStatement()
        {
            _executor.Initialise(@"
                if (1 == 1)
                    var x := 2
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(2, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ExecutesTrueIfStatementWithBlock()
        {
            _executor.Initialise(@"
                var z:= 0
                if (!(1 > 2) and 1 <= 2)
                begin
                    var x:=3
                    x:=4
                    z:=x
                end
            ");
            
            _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("z");

            var expected = new ExpressionValue(4, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
            Assert.False(_programState.IsVariableDefined("x"));
        }
        
        [Fact]
        public void ExecutesSimpleFalseIfStatement()
        {
            _executor.Initialise(@"
                if (1 != 1)
                    var x := 2
            ");
            
            _executor.ExecuteAll();

            Assert.False(_programState.IsVariableDefined("x"));
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
            
            var actual = _programState.GetVariable("x");

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
            
            var actual = _programState.GetVariable("x");

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
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(4, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
    }
}