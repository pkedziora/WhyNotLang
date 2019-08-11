using System.Collections.Generic;
using Xunit;

namespace WhyNotLang.Tokenizer.Tests
{
    public class MultiTokenTests
    {
        private Tokenizer _tokenizer;
        public MultiTokenTests()
        {
            _tokenizer = new Tokenizer(new TokenReader(), new TokenFactory());
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
            yield return new object[] {"123\"abc\"", new[] {new Token(TokenType.Number, "123"), new Token(TokenType.String, "abc")}};
            yield return new object[] {"\"123\"\"abc\"", new[] {new Token(TokenType.String, "123"), new Token(TokenType.String, "abc")}};
            yield return new object[] {"+\"abc\"", new[] {new Token(TokenType.Plus, "+"), new Token(TokenType.String, "abc")}};
            yield return new object[] {"(i<10)", new[] {new Token(TokenType.LeftParen, "("), new Token(TokenType.Identifier, "i"),new Token(TokenType.LessThan, "<"), new Token(TokenType.Number, "10"), new Token(TokenType.RightParen, ")")}};
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
            yield return new object[] {"func123", new[] {new Token(TokenType.Identifier, "func123")}};
            yield return new object[] {"functions", new[] {new Token(TokenType.Identifier, "functions")}};
            yield return new object[] {"abegin", new[] {new Token(TokenType.Identifier, "abegin")}};
        }
        
        [Theory]
        [MemberData(nameof(SplitsKeywordsFromStringsAndOperatorsData))]
        public void SplitsKeywordsFromStringsAndOperators(string tokenStr, Token[] expected)
        {
            var actual = _tokenizer.GetTokens(tokenStr);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> SplitsKeywordsFromStringsAndOperatorsData()
        {
            yield return new object[] {"*func+", new[] {new Token(TokenType.Multiply, "*"), new Token(TokenType.Function, "func"), new Token(TokenType.Plus, "+")}};
        }
        
        [Theory]
        [MemberData(nameof(DetectsInvalidTokensData))]
        public void DetectsInvalidTokens(string tokenStr, Token[] expected)
        {
            var actual = _tokenizer.GetTokens(tokenStr);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> DetectsInvalidTokensData()
        {
            yield return new object[] {"*@/", new[] {new Token(TokenType.Multiply, "*"), new Token(TokenType.Invalid, "@"), new Token(TokenType.Divide, "/")}};
            yield return new object[] {"$func@", new[] {new Token(TokenType.Invalid, "$"), new Token(TokenType.Function, "func"), new Token(TokenType.Invalid, "@")}};
            yield return new object[] {"$function@", new[] {new Token(TokenType.Invalid, "$"), new Token(TokenType.Identifier, "function"), new Token(TokenType.Invalid, "@")}};
            yield return new object[] {"$\"functions\"@", new[] {new Token(TokenType.Invalid, "$"), new Token(TokenType.String, "functions"), new Token(TokenType.Invalid, "@")}};
        }
    }
}