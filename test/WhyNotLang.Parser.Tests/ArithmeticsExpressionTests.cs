using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests
{
    public class ArithmeticsExpressionTests
    {
        private Parser _parser;
        public ArithmeticsExpressionTests()
        {
            _parser = new Parser(new TokenIterator(new Tokenizer.Tokenizer(new TokenReader(), new TokenMap())));
        }

        [Fact]
        public void ParsesSingleNumber()
        {
            var expression = "1";
            var expected = new ValueExpression(new Token(TokenType.Number, "1"));
            
            var result = _parser.ParseExpression(expression);
            var actual = (ValueExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesSingleIdentifier()
        {
            var expression = "a";
            var expected = new ValueExpression(new Token(TokenType.Identifier, "a"));
            
            var result = _parser.ParseExpression(expression);
            var actual = (ValueExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2PartBinaryExpressionWithNumbers()
        {
            var expression = "1 + 2";
            var expected = TestHelpers.GetBinaryExpression(1, "+", 2);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses3PartBinaryExpression()
        {
            var expression = "1 + 2 - 3";
            var inner = TestHelpers.GetBinaryExpression(1, "+", 2);
            var expected = TestHelpers.GetBinaryExpression(inner, "-", 3);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses4PartBinaryExpression()
        {
            var expression = "1 + 2 - 3 + 4";
            var innerInner = TestHelpers.GetBinaryExpression(1, "+", 2);
            var inner = TestHelpers.GetBinaryExpression(innerInner, "-", 3);
            var expected = TestHelpers.GetBinaryExpression(inner, "+", 4);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses3PartBinaryExpressionHighPrecedenceOnLeft()
        {
            var expression = "2 * 3 + 1";
            var inner = TestHelpers.GetBinaryExpression(2, "*", 3);
            var expected = TestHelpers.GetBinaryExpression(inner, "+", 1);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses3PartBinaryExpressionHighPrecedenceOnRight()
        {
            var expression = "1 + 2 * 3";
            var inner = TestHelpers.GetBinaryExpression(2, "*", 3);
            var expected = TestHelpers.GetBinaryExpression(1, "+", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses4PartBinaryExpressionHighPrecedenceInTheMiddle()
        {
            var expression = "1 + 2 * 3 + 4";
            var innerInner = TestHelpers.GetBinaryExpression(2, "*", 3);
            var inner = TestHelpers.GetBinaryExpression(1, "+", innerInner);
            var expected = TestHelpers.GetBinaryExpression(inner, "+", 4);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
    }
}