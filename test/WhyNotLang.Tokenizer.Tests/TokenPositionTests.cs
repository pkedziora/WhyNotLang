using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace WhyNotLang.Tokenizer.Tests
{
    public class TokenPositionTests
    {
        private Tokenizer _tokenizer;
        private ITokenFactory _tokenFactory;
        
        public TokenPositionTests()
        {
            _tokenFactory = new TokenFactory();
            _tokenizer = new Tokenizer(new TokenReader(), _tokenFactory);
        }

        [Fact]
        public void ReturnsTokenPositionsOn1Line()
        {
            var tokenStr = "var x := 1 + 100";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                _tokenFactory.CreateToken(TokenType.Var, null, 1), 
                _tokenFactory.CreateToken(TokenType.Identifier, "x", 1),
                _tokenFactory.CreateToken(TokenType.Assign, null, 1),
                _tokenFactory.CreateToken(TokenType.Number, "1", 1),
                _tokenFactory.CreateToken(TokenType.Plus, null, 1),
                _tokenFactory.CreateToken(TokenType.Number, "100", 1)
            };
            
            Assert.Equal(expected, actual, new TokenWithLineNumbersComparer());
        }

        [Fact]
        public void ReturnsTokenPositionsOnMultipleLines()
        {
            var tokenStr = 
@"if (i <= 10)
begin
    x := 1
end";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                _tokenFactory.CreateToken(TokenType.If, null, 1), 
                _tokenFactory.CreateToken(TokenType.LeftParen, null, 1),
                _tokenFactory.CreateToken(TokenType.Identifier, "i", 1),
                _tokenFactory.CreateToken(TokenType.LessThanOrEqual, null,1),
                _tokenFactory.CreateToken(TokenType.Number, "10", 1),
                _tokenFactory.CreateToken(TokenType.RightParen, null,1),
                _tokenFactory.CreateToken(TokenType.Begin, null, 2),
                _tokenFactory.CreateToken(TokenType.Identifier, "x", 3),
                _tokenFactory.CreateToken(TokenType.Assign, null, 3),
                _tokenFactory.CreateToken(TokenType.Number, "1", 3),
                _tokenFactory.CreateToken(TokenType.End, null, 4)
            };
            
            Assert.Equal(expected, actual, new TokenWithLineNumbersComparer());
        }
    }

    public class TokenWithLineNumbersComparer : IEqualityComparer<Token>
    {
        public bool Equals(Token x, Token y)
        {
            return x.Equals(y) && x.LineNumber == y.LineNumber;
        }

        public int GetHashCode(Token obj)
        {
            throw new System.NotImplementedException();
        }
    }
}