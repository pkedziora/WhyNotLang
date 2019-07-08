using System;
using System.Linq;
using Xunit;

namespace WhyNotLang.Tokenizer.Tests
{
    public class SingleTokenTests
    {
        private Tokenizer _tokenizer;
        public SingleTokenTests()
        {
            _tokenizer = new Tokenizer();
        }
        
        [Theory]
        [InlineData("var", TokenType.Var)]
        [InlineData("begin", TokenType.Begin)]
        [InlineData("end", TokenType.End)]
        [InlineData("if", TokenType.If)]
        [InlineData("else", TokenType.Else)]
        [InlineData("while", TokenType.While)]
        [InlineData("function", TokenType.Function)]
        [InlineData("return", TokenType.Return)]
        [InlineData("=", TokenType.Assign)]
        [InlineData("(", TokenType.LeftParen)]
        [InlineData(")", TokenType.RightParen)]
        [InlineData("+", TokenType.Plus)]
        [InlineData("-", TokenType.Minus)]
        [InlineData("*", TokenType.Multiply)]
        [InlineData("/", TokenType.Divide)]
        [InlineData("<", TokenType.LessThan)]
        [InlineData(">", TokenType.GreaterThan)]
        [InlineData("<=", TokenType.LessThanOrEqual)]
        [InlineData(">=", TokenType.GreaterThanOrEqual)]
        [InlineData("==", TokenType.Equal)]
        [InlineData("!=", TokenType.NotEqual)]
        public void RecognizesSingleTokens(string tokenStr, TokenType expected)
        {
            var actual = _tokenizer.GetTokens(tokenStr);
            var actualToken = actual.First();
            Assert.Equal(1, actual.Count);
            Assert.Equal(expected, actualToken.Type);
        }
    }
}