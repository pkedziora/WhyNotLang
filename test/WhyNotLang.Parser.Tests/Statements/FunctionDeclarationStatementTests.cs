using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Test.Common;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class FunctionDeclarationStatementTests
    {
        private IParser _parser;

        public FunctionDeclarationStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _parser = serviceProvider.GetService<IParser>();
        }

        [Fact]
        public void ParsesFunctionWithoutParamsAndEmptyBody()
        {
            _parser.Initialise(@"
                function foo()
                begin
                end                
            ");
            var expected = new FunctionDeclarationStatement(
                new Token(TokenType.Identifier, "foo"), new List<Token>(), new BlockStatement(new List<IStatement>()));
            
            var actual = _parser.ParseNext();

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesFunctionWith1ParameterAndEmptyBody()
        {
            _parser.Initialise(@"
                function foo(abc)
                begin
                end                
            ");
            var expected = new FunctionDeclarationStatement(
                new Token(TokenType.Identifier, "foo"), 
                new List<Token>() {new Token(TokenType.Identifier, "abc")}, 
                new BlockStatement(new List<IStatement>()));
            
            var actual = _parser.ParseNext();

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesFunctionWith2ParameterAndEmptyBody()
        {
            _parser.Initialise(@"
                function foo(abc,d)
                begin
                end                
            ");
            var expected = new FunctionDeclarationStatement(
                new Token(TokenType.Identifier, "foo"), 
                new List<Token>() {new Token(TokenType.Identifier, "abc"), new Token(TokenType.Identifier, "d")}, 
                new BlockStatement(new List<IStatement>()));
            
            var actual = _parser.ParseNext();

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesFunctionWith3ParameterAndBodySet()
        {
            _parser.Initialise(@"
                function foo(abc,d,e)
                begin
                    x := 1
                    var y := 2
                    var abc := (2 + 2) * 3
                end                
            ");
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
            
            var actual = _parser.ParseNext();

            Assert.Equal(expected, actual);
        }
    }
}