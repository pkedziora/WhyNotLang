using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class VariableAssignmentTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public VariableAssignmentTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = _executor.ProgramState;
        }

        [Fact]
        public async Task ExecutesVariableAssignementWithNumberExpression()
        {
            _executor.Initialise(@"
                var x := 1
                x := 2
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(2, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ExecutesVariableDeclarationWithComplexExpression()
        {
            _executor.Initialise(@"
                var abc:=100
                abc := (10 * (4/4) + (2 - 1))");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("abc");

            var expected = new ExpressionValue(11, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ThrowsDuringVariableAssignementWhenUndeclared()
        {
            _executor.Initialise(@"
                x := 2
            ");

            await Assert.ThrowsAsync<WhyNotLangException>(async () => await _executor.ExecuteAll());
        }
        
        [Fact]
        public async Task ExecutesGlobalVariableAssignementWithNumberExpression()
        {
            _executor.Initialise(@"
                global x := 1
                x := 2
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(2, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task LocalVariableAssignementHidesGlobal()
        {
            _executor.Initialise(@"
                global x := 1
                var x := 2
            ");
            
            await _executor.ExecuteAll();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(2, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
    }
}