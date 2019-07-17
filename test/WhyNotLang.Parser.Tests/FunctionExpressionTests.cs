using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests
{
    public class FunctionExpressionTests
    {
        private Parser _parser;
        public FunctionExpressionTests()
        {
            _parser = new Parser(new TokenIterator(new Tokenizer.Tokenizer(new TokenReader(), new TokenMap())));
        }

        [Fact]
        public void ParsesParameterlessFunction()
        {
            var expression = "foo()";
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), null);
            
            var result = _parser.ParseExpression(expression);
            var actual = (FunctionExpression) result;
            
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Type, actual.Type);
            Assert.Null(actual.Parameter);
        }
        
        [Fact]
        public void Parses1NumberParameterFunction()
        {
            var expression = "foo(1)";
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                new ValueExpression(new Token( TokenType.Number,"1")));
            
            var result = _parser.ParseExpression(expression);
            var actual = (FunctionExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1SimpleExpressionParameterFunction()
        {
            var expression = "foo(1 + 2)";
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                TestHelpers.GetBinaryExpression(1, "+", 2));
            
            var result = _parser.ParseExpression(expression);
            var actual = (FunctionExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1ParenExpressionParameterFunction()
        {
            var expression = "foo(1 + (2 - 3))";
            
            var innerExpression = TestHelpers.GetBinaryExpression(2, "-", 3);
            var expectedExpression = TestHelpers.GetBinaryExpression(1, "+", innerExpression);
            
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                expectedExpression);
            
            var result = _parser.ParseExpression(expression);
            var actual = (FunctionExpression) result;
            
            Assert.Equal(expected, actual);
        }
    }
}