using System.Collections.Generic;
using Xunit;

namespace WhyNotLang.Tokenizer.Tests
{
    public class MultiTokenTests
    {
        private Tokenizer _tokenizer;
        public MultiTokenTests()
        {
            _tokenizer = new Tokenizer();
        }
        
        [Theory]
        [MemberData(nameof(SplitNonKeywordsData))]
        public void SplitsNonKeywords(string tokenStr, Token[] expected)
        {
            var actual = _tokenizer.GetTokens(tokenStr);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> SplitNonKeywordsData()
        {
            yield return new object[] {"+*", new[] {new Token(TokenType.Plus, "+"), new Token(TokenType.Multiply, "*")}};
            yield return new object[] {"(/)", new[] {new Token(TokenType.LeftParen, "("), new Token(TokenType.Divide, "/"), new Token(TokenType.RightParen, ")")}};
        }
        
        [Theory]
        [MemberData(nameof(SplitsNonKeywordsFromIdentifiersAndNumbersData))]
        public void SplitsNonKeywordsFromIdentifiersAndNumbers(string tokenStr, Token[] expected)
        {
            var actual = _tokenizer.GetTokens(tokenStr);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> SplitsNonKeywordsFromIdentifiersAndNumbersData()
        {
            yield return new object[] {"+abc", new[] {new Token(TokenType.Plus, "+"), new Token(TokenType.Identifier, "abc")}};
            yield return new object[] {"/abc*", new[] {new Token(TokenType.Divide, "/"), new Token(TokenType.Identifier, "abc"), new Token(TokenType.Multiply, "*")}};
            yield return new object[] {"+123", new[] {new Token(TokenType.Plus, "+"), new Token(TokenType.Number, "123")}};
            yield return new object[] {"/123*", new[] {new Token(TokenType.Divide, "/"), new Token(TokenType.Number, "123"), new Token(TokenType.Multiply, "*")}};
            yield return new object[] {"123abc", new[] {new Token(TokenType.Number, "123"), new Token(TokenType.Identifier, "abc")}};
        }
        
        [Theory]
        [MemberData(nameof(DoesNotSplitKeywordsFromIdentifiersAndNumbersData))]
        public void DoesNotSplitKeywordsFromIdentifiersAndNumbers(string tokenStr, Token[] expected)
        {
            var actual = _tokenizer.GetTokens(tokenStr);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> DoesNotSplitKeywordsFromIdentifiersAndNumbersData()
        {
            yield return new object[] {"function123", new[] {new Token(TokenType.Identifier, "function123")}};
            yield return new object[] {"functions", new[] {new Token(TokenType.Identifier, "functions")}};
            yield return new object[] {"abegin", new[] {new Token(TokenType.Identifier, "abegin")}};
        }
    }
}