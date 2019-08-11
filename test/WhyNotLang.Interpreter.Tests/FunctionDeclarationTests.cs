using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Test.Common;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Interpreter.Tests
{
    public class FunctionDeclarationTests
    {
        private IProgramState _programState;
        private IExecutor _executor;

        public FunctionDeclarationTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _executor = serviceProvider.GetService<IExecutor>();
            _programState = _executor.ProgramState;
        }

        [Fact]
        public async Task ExecutesFunctionDeclarationWithoutParamsAndEmptyBody()
        {
            _executor.Initialise(@"
                func foo()
                begin
                end                
            ");
            
            await _executor.ExecuteNext();
            
            var expected = new FunctionDeclarationStatement(
                new Token(TokenType.Identifier, "foo"), new List<Token>(), new BlockStatement(new List<IStatement>()));

            var actual = _programState.GetFunction("foo");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ExecutesFunctionDeclarationWith3ParameterAndBodySet()
        {
            _executor.Initialise(@"
                func foo(abc,d,e)
                begin
                    x := 1
                    var y := 2
                    var abc := (2 + 2) * 3
                end                
            ");
            
            await _executor.ExecuteNext();
            
            var expected = new FunctionDeclarationStatement(
                new Token(TokenType.Identifier, "foo"), 
                new List<Token>() {new Token(TokenType.Identifier, "abc"), new Token(TokenType.Identifier, "d"), new Token(TokenType.Identifier, "e")}, 
                new BlockStatement(new List<IStatement>
                {
                    TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1)),
                    TestHelpers.GetVariableDeclarationStatement("y", TestHelpers.GetValueExpression(2)),
                    TestHelpers.GetVariableDeclarationStatement("abc", TestHelpers.GetBinaryExpression(
                        TestHelpers.GetBinaryExpression(2, "+", 2), "*", 3))
                }));
            
            var actual = _programState.GetFunction("foo");

            Assert.Equal(expected, actual);
        }
    }
}