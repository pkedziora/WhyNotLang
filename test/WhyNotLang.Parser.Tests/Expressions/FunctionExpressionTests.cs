using System.Collections.Generic;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Expressions
{
    public class FunctionExpressionTests
    {
        private readonly ExpressionParser _parser;
        public FunctionExpressionTests()
        {
            _parser = TestHelpers.CreateExpressionParser();
        }

        [Fact]
        public void ParsesParameterlessFunction()
        {
            var expression = "foo()";
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), new EmptyExpression());
            
            var result = _parser.ParseExpression(expression);
            var actual = (FunctionExpression) result;
            
            Assert.Equal(expected, actual);
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
        public void Parses1StringParameterFunction()
        {
            var expression = "foo(\"abc\")";
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                new ValueExpression(new Token( TokenType.String,"abc")));
            
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
        public void Parses1SimpleExpressionWithStringsParameterFunction()
        {
            var expression = "foo(\"abc\" + \"def\")";
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                TestHelpers.GetBinaryExpressionWithStrings("abc", "+", "def"));
            
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
        
        [Fact]
        public void Parses1SimpleExpressionParameterFunctionPrecededByExpression()
        {
            var expression = "3 + foo(1 + 2)";
            
            var functionExp = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                TestHelpers.GetBinaryExpression(1, "+", 2));
            
            var expected = TestHelpers.GetBinaryExpression(3, "+", functionExp);

            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParsesEmptyExpressionParameterFunctionFollowedByExpression()
        {
            var expression = "foo() - 4";
            
            var functionExp = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                new EmptyExpression());
            
            var expected = TestHelpers.GetBinaryExpression(functionExp, "-", 4);

            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesValueExpressionParameterFunctionFollowedByExpression()
        {
            var expression = "foo(1) - 4";
            
            var functionExp = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                new ValueExpression(new Token(TokenType.Number, "1")));
            
            var expected = TestHelpers.GetBinaryExpression(functionExp, "-", 4);

            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1SimpleExpressionParameterFunctionFollowedByExpression()
        {
            var expression = "foo(1 + 2) - 4";
            
            var functionExp = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                TestHelpers.GetBinaryExpression(1, "+", 2));
            
            var expected = TestHelpers.GetBinaryExpression(functionExp, "-", 4);

            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1SimpleExpressionParameterFunctionAsPartOfExpression()
        {
            var expression = "3 + foo(1 + 2) - 4";
            
            var functionExp = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                TestHelpers.GetBinaryExpression(1, "+", 2));

            var innerExp = TestHelpers.GetBinaryExpression(3, "+", functionExp);
            var expected = TestHelpers.GetBinaryExpression(innerExp, "-", 4);

            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2NumberParameterFunction()
        {
            var expression = "foo(1,2)";
            var expectedParameters = new List<IExpression>
            {
                new ValueExpression(new Token(TokenType.Number, "1")),
                new ValueExpression(new Token(TokenType.Number, "2")),
            };
            
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                expectedParameters);
            
            var result = _parser.ParseExpression(expression);
            var actual = (FunctionExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2SimpleExpressionParameterFunction()
        {
            var expression = "foo(1 + 2, 3 * 4)";
            
            var expectedParameters = new List<IExpression>
            {
                TestHelpers.GetBinaryExpression(1, "+", 2),
                TestHelpers.GetBinaryExpression(3, "*", 4)
            };
            
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                expectedParameters);
            
            var result = _parser.ParseExpression(expression);
            var actual = (FunctionExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses3SimpleExpressionParameterFunction()
        {
            var expression = "foo(1 + 2, 5, 3 * 4)";
            
            var expectedParameters = new List<IExpression>
            {
                TestHelpers.GetBinaryExpression(1, "+", 2),
                new ValueExpression(new Token(TokenType.Number, "5")),
                TestHelpers.GetBinaryExpression(3, "*", 4)
            };
            
            var expected = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                expectedParameters);
            
            var result = _parser.ParseExpression(expression);
            var actual = (FunctionExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2NumberParameterFunctionFollowedByExpression()
        {
            var expression = "foo(1,2) + 1";
            var expectedParameters = new List<IExpression>
            {
                new ValueExpression(new Token(TokenType.Number, "1")),
                new ValueExpression(new Token(TokenType.Number, "2")),
            };
            
            var functionExpression = new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                expectedParameters);
            var expected = TestHelpers.GetBinaryExpression(functionExpression, "+", 1);
            
            var result = _parser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
    }
}