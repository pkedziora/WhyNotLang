using System.Collections.Generic;
using Xunit;

namespace WhyNotLang.Tokenizer.Tests
{
    public class WhitespaceTests
    {
        private Tokenizer _tokenizer;
        public WhitespaceTests()
        {
            _tokenizer = new Tokenizer(new TokenReader(), new TokenFactory());
        }
        
        [Theory]
        [MemberData(nameof(RemovesLeadingAndTrailingWhitespaceData))]
        public void RemovesLeadingAndTrailingWhitespace(string tokenStr, Token[] expected)
        {
            var actual = _tokenizer.GetTokens(tokenStr);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> RemovesLeadingAndTrailingWhitespaceData()
        {
            yield return new object[] {" *( ", new[] {new Token(TokenType.Multiply, "*"), new Token(TokenType.LeftParen, "(")}};
            yield return new object[] {" function ", new[] {new Token(TokenType.Function, "function")}};
            yield return new object[] {"    functions    ", new[] {new Token(TokenType.Identifier, "functions")}};
            yield return new object[] {"    *(    ", new[] {new Token(TokenType.Multiply, "*"), new Token(TokenType.LeftParen, "(")}};
            yield return new object[] {"    \"a b    c\"    ", new[] {new Token(TokenType.String, "a b    c")}};
        }
        
        [Theory]
        [MemberData(nameof(RemovesMiddleWhitespaceData))]
        public void RemovesMiddleWhitespace(string tokenStr, Token[] expected)
        {
            var actual = _tokenizer.GetTokens(tokenStr);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> RemovesMiddleWhitespaceData()
        {
            yield return new object[] {"* (", new[] {new Token(TokenType.Multiply, "*"), new Token(TokenType.LeftParen, "(")}};
            yield return new object[] {"function    *", new[] {new Token(TokenType.Function, "function"), new Token(TokenType.Multiply, "*")}};
            yield return new object[] {"*   functions", new[] {new Token(TokenType.Multiply, "*"), new Token(TokenType.Identifier, "functions")}};
            yield return new object[] {"    *    ( +", new[] {new Token(TokenType.Multiply, "*"), new Token(TokenType.LeftParen, "("), new Token(TokenType.Plus, "+")}};
            yield return new object[] {"+    \"a b    c\"    ", new[] {new Token(TokenType.Plus, "+"), new Token(TokenType.String, "a b    c")}};
        }
    }
}