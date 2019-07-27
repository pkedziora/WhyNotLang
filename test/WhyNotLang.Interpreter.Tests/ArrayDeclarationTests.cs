using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
using WhyNotLang.Interpreter.State;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class ArrayDeclarationTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public ArrayDeclarationTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = serviceProvider.GetService<IProgramState>();
        }

        [Fact]
        public void ExecutesArrayDeclarationWithNumberExpression()
        {
            _executor.Initialise("var x[10]");
            
            _executor.ExecuteNext();
            
            var actual = _programState.IsArrayDefined("x");
            var actualSize = _programState.CurrentScope.Arrays["x"].Length;

            Assert.True(actual);
            Assert.Equal(10, actualSize);
        }
        
        [Fact]
        public void ExecutesArrayDeclarationWithArithmeticExpressionAsSize()
        {
            _executor.Initialise("var x[2 + 2 * 4]");
            
            _executor.ExecuteNext();
            
            var actual = _programState.IsArrayDefined("x");
            var actualSize = _programState.CurrentScope.Arrays["x"].Length;

            Assert.True(actual);
            Assert.Equal(10, actualSize);
        }
        
        [Fact]
        public void ExecutesGlobalArrayDeclarationWithNumberExpression()
        {
            _executor.Initialise("global x[10]");
            
            _executor.ExecuteNext();
            
            var actual = _programState.IsArrayDefined("x");
            var actualSize = _programState.GlobalScope.Arrays["x"].Length;

            Assert.True(actual);
            Assert.Equal(10, actualSize);
        }
    }
}