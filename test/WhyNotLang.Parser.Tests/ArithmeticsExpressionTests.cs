using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests
{
    public class ArithmeticsExpressionTests
    {
        private ExpressionParser _parser;
        public ArithmeticsExpressionTests()
        {
            _parser = new ExpressionParser(new TokenIterator(new Tokenizer.Tokenizer(new TokenReader(), new TokenMap())));
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
        public void Parses2PartBinaryExpressionWithIdentifier()
        {
            var expression = "a + b";
            var expected = TestHelpers.GetBinaryExpression("a", "+", "b");
            
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
        public void Parses4PartBinaryExpressionWithNumbersAndIdentifiers()
        {
            var expression = "a + b - 3 + 4";
            var innerInner = TestHelpers.GetBinaryExpression("a", "+", "b");
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
        
        [Fact]
        public void Parses4PartBinaryExpressionLowPrecedenceInTheMiddle()
        {
            var expression = "1 * 2 + 3 / 4";
            var leftInner = TestHelpers.GetBinaryExpression(1, "*", 2);
            var rightInner = TestHelpers.GetBinaryExpression(3, "/", 4);
            var expected = TestHelpers.GetBinaryExpression(leftInner, "+", rightInner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1PartUnaryExpression()
        {
            var expression = "-1";
            var expected = new UnaryExpression(new ValueExpression(TestHelpers.GetToken("1")), TestHelpers.GetToken("-"));
            
            var result = _parser.ParseExpression(expression);
            var actual = (UnaryExpression) result;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2PartUnaryExpression()
        {
            var expression = "+-1";
            var expected = new UnaryExpression(
                new UnaryExpression(
                    new ValueExpression(TestHelpers.GetToken("1")), TestHelpers.GetToken("-")), 
                TestHelpers.GetToken("+"));
            
            var result = _parser.ParseExpression(expression);
            var actual = (UnaryExpression) result;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesRedundantSingleParens()
        {
            var expression = "(1)";
            var expected = new ValueExpression(TestHelpers.GetToken("1"));
            
            var result = _parser.ParseExpression(expression);
            var actual = (ValueExpression) result;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesRedundantSingleParensWithNegativeNumber()
        {
            var expression = "(-1)";
            var expected = TestHelpers.GetUnaryExpression("-", 1);
            
            var result = _parser.ParseExpression(expression);
            var actual = (UnaryExpression) result;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesMinusInFrontOfSingleParens()
        {
            var expression = "-(1)";
            var expected = TestHelpers.GetUnaryExpression("-", 1);
            
            var result = _parser.ParseExpression(expression);
            var actual = (UnaryExpression) result;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesRedundantParensAround3PartBinaryExpression()
        {
            var expression = "(1 + 2) - 3";
            var inner = TestHelpers.GetBinaryExpression(1, "+", 2);
            var expected = TestHelpers.GetBinaryExpression(inner, "-", 3);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesMinusInFrontOfParensWithExpression()
        {
            var expression = "-(1 + 2) - 3";
            var inner = TestHelpers.GetUnaryExpression("-", TestHelpers.GetBinaryExpression(1, "+", 2));
            var expected = TestHelpers.GetBinaryExpression(inner, "-", 3);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesRedundantParensAround3PartBinaryExpressionWithNegativeNumber()
        {
            var expression = "(-1 + -2) - 3";
            
            var inner = TestHelpers.GetBinaryExpression(TestHelpers.GetUnaryExpression("-", 1), "+", TestHelpers.GetUnaryExpression("-", 2));
            var expected = TestHelpers.GetBinaryExpression(inner, "-", 3);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesParensAround3PartBinaryExpression()
        {
            var expression = "1 + (2 - 3)";
            var inner = TestHelpers.GetBinaryExpression(2, "-", 3);
            var expected = TestHelpers.GetBinaryExpression(1, "+", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesMinusInFrontInParensAround3PartBinaryExpression()
        {
            var expression = "1 * -(2 - 3)";
            var inner = TestHelpers.GetUnaryExpression("-",TestHelpers.GetBinaryExpression(2, "-", 3));
            var expected = TestHelpers.GetBinaryExpression(1, "*", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesParensAround3PartBinaryExpressionWithNegativeNumber()
        {
            var expression = "1 + (2 - -3)";
            var inner = TestHelpers.GetBinaryExpression(2, "-", TestHelpers.GetUnaryExpression("-", 3));
            var expected = TestHelpers.GetBinaryExpression(1, "+", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesParensAround4PartBinaryExpression()
        {
            var expression = "1 * (2 + 3) * 4";
            var innerInner = TestHelpers.GetBinaryExpression(2, "+", 3);
            var inner = TestHelpers.GetBinaryExpression(1, "*", innerInner);
            var expected = TestHelpers.GetBinaryExpression(inner, "*", 4);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesParensAround4PartBinaryExpressionRightAssociative()
        {
            var expression = "1 + (2 + 3) * 4";
            var innerInner = TestHelpers.GetBinaryExpression(2, "+", 3);
            var inner = TestHelpers.GetBinaryExpression(innerInner, "*", 4);
            var expected = TestHelpers.GetBinaryExpression(1, "+", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesSomeRedundantParensAround4PartBinaryExpression()
        {
            var expression = "((1 * (2 + 3)) * 4)";
            var innerInner = TestHelpers.GetBinaryExpression(2, "+", 3);
            var inner = TestHelpers.GetBinaryExpression(1, "*", innerInner);
            var expected = TestHelpers.GetBinaryExpression(inner, "*", 4);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
    }
}