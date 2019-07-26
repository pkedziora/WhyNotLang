using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Test.Common;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class ArrayAssignmentStatementTests
    {
        private IParser _parser;
        public ArrayAssignmentStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _parser = serviceProvider.GetService<IParser>();
        }

        [Fact]
        public void ParsesArrayAssignmentWithSingleNumber()
        {
            _parser.Initialise("x[0] := 1");
            var expected = new ArrayAssignmentStatement(
                new Token(TokenType.Identifier, "x"),
                new ValueExpression(new Token(TokenType.Number, "0")),
                new ValueExpression(new Token(TokenType.Number, "1")));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesArrayAssignmentWithString()
        {
            _parser.Initialise("x[0] := \"abc\"");
            var expected = new ArrayAssignmentStatement(
                new Token(TokenType.Identifier, "x"),
                new ValueExpression(new Token(TokenType.Number, "0")),
                new ValueExpression(new Token(TokenType.String, "abc")));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesArrayAssignmentWith2PartExpressions()
        {
            _parser.Initialise("x[0 + 1] := 2 + 3");
            var expected = new ArrayAssignmentStatement(
                new Token(TokenType.Identifier, "x"),
                TestHelpers.GetBinaryExpression(0, "+", 1),
                TestHelpers.GetBinaryExpression(2, "+", 3));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesArrayAssignmentWith2PartExpressionWithIdentifiers()
        {
            _parser.Initialise("x[a * b] := c + d");
            var expected = new ArrayAssignmentStatement(
                new Token(TokenType.Identifier, "x"),
                TestHelpers.GetBinaryExpressionWithIdentifiers("a", "*", "b"),
                TestHelpers.GetBinaryExpressionWithIdentifiers("c", "+", "d"));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesArrayAssignmentWith3PartExpression()
        {
            _parser.Initialise("x[0] := 1 + 2 * 3");
            var inner = TestHelpers.GetBinaryExpression(2, "*", 3);
            var expression = TestHelpers.GetBinaryExpression(1, "+", inner);
            var expected = new ArrayAssignmentStatement(
                new Token(TokenType.Identifier, "x"),
                new ValueExpression(new Token(TokenType.Number, "0")), 
                expression);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesArrayAssignmentWith3PartExpressionWithParens()
        {
            _parser.Initialise("x[0] := (1 + 2) * 3");
            var inner = TestHelpers.GetBinaryExpression(1, "+", 2);
            var expression = TestHelpers.GetBinaryExpression(inner, "*", 3);
            var expected = new ArrayAssignmentStatement(
                new Token(TokenType.Identifier, "x"),
                new ValueExpression(new Token(TokenType.Number, "0")),
                expression);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
    }
}