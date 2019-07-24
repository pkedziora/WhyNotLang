using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
using WhyNotLang.Interpreter;
using WhyNotLang.Interpreter.State;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Test.Common;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class FunctionCallStatementTests
    {
        private IParser _parser;

        public FunctionCallStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _parser = serviceProvider.GetService<IParser>();
        }
        
        [Fact]
        public void ParsesParameterlessFunctionStatement()
        {
            _parser.Initialise("foo()");
            var expected = new FunctionCallStatement(new FunctionExpression(new Token(TokenType.Identifier, "foo"), new EmptyExpression()));
            
            var actual = _parser.ParseNext();

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1SimpleExpressionParameterFunctionStatement()
        {
            _parser.Initialise("foo(1 + 2)");
            var expected = new FunctionCallStatement(new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                TestHelpers.GetBinaryExpression(1, "+", 2)));
            
            var actual = _parser.ParseNext();

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2NumberParameterFunctionStatement()
        {
            _parser.Initialise("foo(1,2)");
            var expectedParameters = new List<IExpression>
            {
                new ValueExpression(new Token(TokenType.Number, "1")),
                new ValueExpression(new Token(TokenType.Number, "2")),
            };
            
            var expected = new FunctionCallStatement(new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                expectedParameters));
            
            var actual = _parser.ParseNext();

            Assert.Equal(expected, actual);
        }
    }
}