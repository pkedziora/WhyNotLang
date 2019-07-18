using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests
{
    public class BooleanExpressionTests
    {
        private Parser _parser;
        public BooleanExpressionTests()
        {
            _parser = new Parser(new TokenIterator(new Tokenizer.Tokenizer(new TokenReader(), new TokenMap())));
        }

        [Fact]
        public void Parses2PartBooleanExpressionWithLessThan()
        {
            var expression = "1 < 2";
            var expected = TestHelpers.GetBinaryExpression(1, "<", 2);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2PartBooleanExpressionWithLessThanOrEqual()
        {
            var expression = "1 <= 2";
            var expected = TestHelpers.GetBinaryExpression(1, "<=", 2);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2PartBooleanExpressionWithNegativeNumbers()
        {
            var expression = "-1 <= -2";
            var expected = TestHelpers.GetBinaryExpression(-1, "<=", -2);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2PartBooleanExpression()
        {
            var expression = "1 == 2 AND 4 > 3";
            var left = TestHelpers.GetBinaryExpression(1, "==", 2);
            var right = TestHelpers.GetBinaryExpression(4, ">", 3);
            var expected = TestHelpers.GetBinaryExpression(left, "AND", right);
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses3PartBinaryExpressionWithSamePrecedence()
        {
            var expression = "1 >= 2 OR 3 < 4 OR 5 <= 6";
            var left = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var middle = TestHelpers.GetBinaryExpression(3, "<", 4);
            var right = TestHelpers.GetBinaryExpression(5, "<=", 6);

            var inner = TestHelpers.GetBinaryExpression(left, "OR", middle);
            
            var expected = TestHelpers.GetBinaryExpression(inner, "OR", right);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses3PartBooleanExpressionHighPrecedenceOnRight()
        {
            var expression = "1 >= 2 OR 3 < 4 AND 5 <= 6";
            var left = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var middle = TestHelpers.GetBinaryExpression(3, "<", 4);
            var right = TestHelpers.GetBinaryExpression(5, "<=", 6);

            var inner = TestHelpers.GetBinaryExpression(middle, "AND", right);
            
            var expected = TestHelpers.GetBinaryExpression(left, "OR", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Parses4PartBooleanExpressionHighPrecedenceInTheMiddle()
        {
            var expression = "1 >= 2 OR 3 < 4 AND 5 <= 6 OR 7 == 8";
            var first = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var second = TestHelpers.GetBinaryExpression(3, "<", 4);
            var third = TestHelpers.GetBinaryExpression(5, "<=", 6);
            var fourth = TestHelpers.GetBinaryExpression(7, "==", 8);
            
            var innerInner = TestHelpers.GetBinaryExpression(second, "AND", third);
            
            var inner = TestHelpers.GetBinaryExpression(first, "OR", innerInner);
            var expected = TestHelpers.GetBinaryExpression(inner, "OR", fourth);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesRedundantSingleParens()
        {
            var expression = "(1 < 2)";
            var expected = TestHelpers.GetBinaryExpression(1, "<", 2);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesRedundantParensAround2PartBooleanExpression()
        {
            var expression = "((1 == 2) AND (4 > 3))";
            var left = TestHelpers.GetBinaryExpression(1, "==", 2);
            var right = TestHelpers.GetBinaryExpression(4, ">", 3);
            var expected = TestHelpers.GetBinaryExpression(left, "AND", right);
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesParensAround3PartBooleanExpression()
        {
            var expression = "1 >= 2 AND (3 < 4 OR 5 <= 6)";
            var left = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var middle = TestHelpers.GetBinaryExpression(3, "<", 4);
            var right = TestHelpers.GetBinaryExpression(5, "<=", 6);

            var inner = TestHelpers.GetBinaryExpression(middle, "OR", right);
            
            var expected = TestHelpers.GetBinaryExpression(left, "AND", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesParensAround4PartBooleanExpression()
        {
            var expression = "1 >= 2 OR (3 < 4 OR 5 <= 6) AND 7 == 8";
            var first = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var second = TestHelpers.GetBinaryExpression(3, "<", 4);
            var third = TestHelpers.GetBinaryExpression(5, "<=", 6);
            var fourth = TestHelpers.GetBinaryExpression(7, "==", 8);
            
            var innerInner = TestHelpers.GetBinaryExpression(second, "OR", third);
            
            var inner = TestHelpers.GetBinaryExpression(innerInner, "AND", fourth);
            var expected = TestHelpers.GetBinaryExpression(first, "OR", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
    }
}