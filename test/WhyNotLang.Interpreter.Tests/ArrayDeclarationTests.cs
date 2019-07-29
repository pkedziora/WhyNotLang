using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
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
            _programState = _executor.ProgramState;
        }

        [Fact]
        public async Task ExecutesArrayDeclarationWithNumberExpression()
        {
            _executor.Initialise("var x[10]");
            
            await _executor.ExecuteNext();
            
            var actual = _programState.IsArrayDefined("x");
            var actualSize = _programState.CurrentScope.Arrays["x"].Length;

            Assert.True(actual);
            Assert.Equal(10, actualSize);
        }
        
        [Fact]
        public async Task ExecutesArrayDeclarationWithArithmeticExpressionAsSize()
        {
            _executor.Initialise("var x[2 + 2 * 4]");
            
            await _executor.ExecuteNext();
            
            var actual = _programState.IsArrayDefined("x");
            var actualSize = _programState.CurrentScope.Arrays["x"].Length;

            Assert.True(actual);
            Assert.Equal(10, actualSize);
        }
        
        [Fact]
        public async Task ExecutesGlobalArrayDeclarationWithNumberExpression()
        {
            _executor.Initialise("global x[10]");
            
            await _executor.ExecuteNext();
            
            var actual = _programState.IsArrayDefined("x");
            var actualSize = _programState.GlobalScope.Arrays["x"].Length;

            Assert.True(actual);
            Assert.Equal(10, actualSize);
        }
    }
}