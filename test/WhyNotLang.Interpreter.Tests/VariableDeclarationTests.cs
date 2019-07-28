using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WhyNotLang.Interpreter.Evaluators.ExpressionValues;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class VariableDeclarationTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public VariableDeclarationTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = serviceProvider.GetService<IProgramState>();
        }

        [Fact]
        public async Task ExecutesVariableDeclarationWithNumberExpression()
        {
            _executor.Initialise("var x := 1");
            
            await _executor.ExecuteNext();
            
            var actual = _programState.GetVariable("x");

            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ExecutesVariableDeclarationWithComplexExpression()
        {
            _executor.Initialise("var abc := (10 * (4/4) + (2 - 1))");
            
            await _executor.ExecuteNext();
            
            var actual = _programState.GetVariable("abc");

            var expected = new ExpressionValue(11, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task ExecutesGlobalVariableDeclarationWithNumberExpression()
        {
            _executor.Initialise("global x := 1");
            
            await _executor.ExecuteNext();
            
            var actual = _programState.GlobalScope.VariableValues["x"];
            var expected = new ExpressionValue(1, ExpressionValueTypes.Number);
            
            Assert.Equal(expected, actual);
        }
    }
}