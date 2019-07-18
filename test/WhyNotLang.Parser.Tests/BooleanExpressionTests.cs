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
            var expected = TestHelpers.GetBinaryExpression(TestHelpers.GetUnaryExpression("-", 1), "<=", TestHelpers.GetUnaryExpression("-", 2));
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2PartBooleanExpression()
        {
            var expression = "1 == 2 and 4 > 3";
            var left = TestHelpers.GetBinaryExpression(1, "==", 2);
            var right = TestHelpers.GetBinaryExpression(4, ">", 3);
            var expected = TestHelpers.GetBinaryExpression(left, "and", right);
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses3PartBinaryExpressionWithSamePrecedence()
        {
            var expression = "1 >= 2 or 3 < 4 or 5 <= 6";
            var left = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var middle = TestHelpers.GetBinaryExpression(3, "<", 4);
            var right = TestHelpers.GetBinaryExpression(5, "<=", 6);

            var inner = TestHelpers.GetBinaryExpression(left, "or", middle);
            
            var expected = TestHelpers.GetBinaryExpression(inner, "or", right);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses3PartBooleanExpressionHighPrecedenceOnRight()
        {
            var expression = "1 >= 2 or 3 < 4 and 5 <= 6";
            var left = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var middle = TestHelpers.GetBinaryExpression(3, "<", 4);
            var right = TestHelpers.GetBinaryExpression(5, "<=", 6);

            var inner = TestHelpers.GetBinaryExpression(middle, "and", right);
            
            var expected = TestHelpers.GetBinaryExpression(left, "or", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Parses4PartBooleanExpressionHighPrecedenceInTheMiddle()
        {
            var expression = "1 >= 2 or 3 < 4 and 5 <= 6 or 7 == 8";
            var first = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var second = TestHelpers.GetBinaryExpression(3, "<", 4);
            var third = TestHelpers.GetBinaryExpression(5, "<=", 6);
            var fourth = TestHelpers.GetBinaryExpression(7, "==", 8);
            
            var innerInner = TestHelpers.GetBinaryExpression(second, "and", third);
            
            var inner = TestHelpers.GetBinaryExpression(first, "or", innerInner);
            var expected = TestHelpers.GetBinaryExpression(inner, "or", fourth);
            
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
        public void ParsesNotInFrontOfSingleParens()
        {
            var expression = "!(1 < 2)";
            var expected = TestHelpers.GetUnaryExpression("!",TestHelpers.GetBinaryExpression(1, "<", 2));
            
            var result = _parser.ParseExpression(expression);
            var actual = (UnaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesRedundantParensAround2PartBooleanExpression()
        {
            var expression = "((1 == 2) and (4 > 3))";
            var left = TestHelpers.GetBinaryExpression(1, "==", 2);
            var right = TestHelpers.GetBinaryExpression(4, ">", 3);
            var expected = TestHelpers.GetBinaryExpression(left, "and", right);
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesNotInFrontOfSecondParensIn2PartBooleanExpression()
        {
            var expression = "((1 == 2) and !(4 > 3))";
            var left = TestHelpers.GetBinaryExpression(1, "==", 2);
            var right = TestHelpers.GetUnaryExpression("!",TestHelpers.GetBinaryExpression(4, ">", 3));
            var expected = TestHelpers.GetBinaryExpression(left, "and", right);
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesNotInFrontOfSecondParensIn2PartBooleanExpressionWithIdentifiers()
        {
            var expression = "((foo == bar) and !(x1 > x3))";
            var left = TestHelpers.GetBinaryExpression("foo", "==", "bar");
            var right = TestHelpers.GetUnaryExpression("!",TestHelpers.GetBinaryExpression("x1", ">", "x3"));
            var expected = TestHelpers.GetBinaryExpression(left, "and", right);
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesParensAround3PartBooleanExpression()
        {
            var expression = "1 >= 2 and (3 < 4 or 5 <= 6)";
            var left = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var middle = TestHelpers.GetBinaryExpression(3, "<", 4);
            var right = TestHelpers.GetBinaryExpression(5, "<=", 6);

            var inner = TestHelpers.GetBinaryExpression(middle, "or", right);
            
            var expected = TestHelpers.GetBinaryExpression(left, "and", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesParensWithMinusInFrontIn3PartBooleanExpression()
        {
            var expression = "1 >= 2 and -(3 < 4 or 5 <= 6)";
            var left = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var middle = TestHelpers.GetBinaryExpression(3, "<", 4);
            var right = TestHelpers.GetBinaryExpression(5, "<=", 6);

            var inner = TestHelpers.GetUnaryExpression("-", TestHelpers.GetBinaryExpression(middle, "or", right));
            
            var expected = TestHelpers.GetBinaryExpression(left, "and", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesParensAround4PartBooleanExpression()
        {
            var expression = "1 >= 2 or (3 < 4 or 5 <= 6) and 7 == 8";
            var first = TestHelpers.GetBinaryExpression(1, ">=", 2);
            var second = TestHelpers.GetBinaryExpression(3, "<", 4);
            var third = TestHelpers.GetBinaryExpression(5, "<=", 6);
            var fourth = TestHelpers.GetBinaryExpression(7, "==", 8);
            
            var innerInner = TestHelpers.GetBinaryExpression(second, "or", third);
            
            var inner = TestHelpers.GetBinaryExpression(innerInner, "and", fourth);
            var expected = TestHelpers.GetBinaryExpression(first, "or", inner);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
    }
}