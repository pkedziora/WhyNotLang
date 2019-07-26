using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Cmd;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Test.Common;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Expressions
{
    public class ArrayExpressionTests
    {
        private readonly IExpressionParser _expressionParser;
        public ArrayExpressionTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _expressionParser = serviceProvider.GetService<IExpressionParser>();
        }

        [Fact]
        public void Parses1NumberParameterArray()
        {
            var expression = "bar[1]";
            var expected = new ArrayExpression(new Token(TokenType.Identifier, "bar"), 
                new ValueExpression(new Token( TokenType.Number,"1")));
            
            var result = _expressionParser.ParseExpression(expression);
            var actual = (ArrayExpression) result;
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Parses1SimpleExpressionParameterArray()
        {
            var expression = "bar[1 + 2]";
            var expected = new ArrayExpression(new Token(TokenType.Identifier, "bar"), 
                TestHelpers.GetBinaryExpression(1, "+", 2));
            
            var result = _expressionParser.ParseExpression(expression);
            var actual = (ArrayExpression) result;
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Parses1ParenExpressionParameterArray()
        {
            var expression = "bar[1 + (2 - 3)]";
            
            var innerExpression = TestHelpers.GetBinaryExpression(2, "-", 3);
            var expectedExpression = TestHelpers.GetBinaryExpression(1, "+", innerExpression);
            
            var expected = new ArrayExpression(new Token(TokenType.Identifier, "bar"), 
                expectedExpression);
            
            var result = _expressionParser.ParseExpression(expression);
            var actual = (ArrayExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1SimpleExpressionParameterArrayPrecededByExpression()
        {
            var expression = "3 + bar[1 + 2]";
            
            var arrayExpression = new ArrayExpression(new Token(TokenType.Identifier, "bar"), 
                TestHelpers.GetBinaryExpression(1, "+", 2));
            
            var expected = TestHelpers.GetBinaryExpression(3, "+", arrayExpression);

            var result = _expressionParser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParsesValueExpressionParameterArrayFollowedByExpression()
        {
            var expression = "bar[1] - 4";
            
            var arrayExpression = new ArrayExpression(new Token(TokenType.Identifier, "bar"), 
                new ValueExpression(new Token(TokenType.Number, "1")));
            
            var expected = TestHelpers.GetBinaryExpression(arrayExpression, "-", 4);

            var result = _expressionParser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1SimpleExpressionParameterArrayFollowedByExpression()
        {
            var expression = "bar[1 + 2] - 4";
            
            var arrayExpression = new ArrayExpression(new Token(TokenType.Identifier, "bar"), 
                TestHelpers.GetBinaryExpression(1, "+", 2));
            
            var expected = TestHelpers.GetBinaryExpression(arrayExpression, "-", 4);

            var result = _expressionParser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1SimpleExpressionParameterArrayAsPartOfExpression()
        {
            var expression = "3 + bar[1 + 2] - 4";
            
            var arrayExpression = new ArrayExpression(new Token(TokenType.Identifier, "bar"), 
                TestHelpers.GetBinaryExpression(1, "+", 2));

            var innerExp = TestHelpers.GetBinaryExpression(3, "+", arrayExpression);
            var expected = TestHelpers.GetBinaryExpression(innerExp, "-", 4);

            var result = _expressionParser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParsesArrayInTheMiddleWithHighPriorityOnTheRight()
        {
            var expression = "1 + bar[2] * 3";

            var arrayExpression = new ArrayExpression(new Token(TokenType.Identifier, "bar"), 
                new ValueExpression(new Token(TokenType.Number, "2")));
            var right = TestHelpers.GetBinaryExpression(arrayExpression, "*", 3);
            var expected = TestHelpers.GetBinaryExpression(1, "+", right);
            
            var result = _expressionParser.ParseExpression(expression);
            var actual = (BinaryExpression) result;
            
            Assert.Equal(expected, actual);
        }
    }
}