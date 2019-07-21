using Xunit;

namespace WhyNotLang.Tokenizer.Tests
{
    public class FullProgramTests
    {
        private Tokenizer _tokenizer;
        private ITokenMap _tokenMap;
        
        public FullProgramTests()
        {
            _tokenMap = new TokenMap();
            _tokenizer = new Tokenizer(new TokenReader(), _tokenMap);
        }
        
        [Fact]
        public void TokenizesHelloWorld()
        {
            var tokenStr = "print(\"Hello World\")";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                _tokenMap.CreateToken(TokenType.Identifier, "print"), 
                _tokenMap.CreateToken(TokenType.LeftParen),
                _tokenMap.CreateToken(TokenType.String, "Hello World"),
                _tokenMap.CreateToken(TokenType.RightParen),
            };
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TokenizesVariableAssignment()
        {
            var tokenStr = "var x := 1 + 100";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                _tokenMap.CreateToken(TokenType.Var), 
                _tokenMap.CreateToken(TokenType.Identifier, "x"),
                _tokenMap.CreateToken(TokenType.Assign),
                _tokenMap.CreateToken(TokenType.Number, "1"),
                _tokenMap.CreateToken(TokenType.Plus),
                _tokenMap.CreateToken(TokenType.Number, "100")
            };
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TokenizesMathExpression()
        {
            var tokenStr = "var x := 2 * (1 + (100 / 2))";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                _tokenMap.CreateToken(TokenType.Var), 
                _tokenMap.CreateToken(TokenType.Identifier, "x"),
                _tokenMap.CreateToken(TokenType.Assign),
                _tokenMap.CreateToken(TokenType.Number, "2"),
                _tokenMap.CreateToken(TokenType.Multiply),
                _tokenMap.CreateToken(TokenType.LeftParen),
                _tokenMap.CreateToken(TokenType.Number, "1"),
                _tokenMap.CreateToken(TokenType.Plus),
                _tokenMap.CreateToken(TokenType.LeftParen),
                _tokenMap.CreateToken(TokenType.Number, "100"),
                _tokenMap.CreateToken(TokenType.Divide),
                _tokenMap.CreateToken(TokenType.Number, "2"),
                _tokenMap.CreateToken(TokenType.RightParen),
                _tokenMap.CreateToken(TokenType.RightParen),
            };
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TokenizesIfStatement()
        {
            var tokenStr = @"
                if (i <= 10)
                begin
                    print(""i is less than or equal 10"")
                end
                ";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                _tokenMap.CreateToken(TokenType.If), 
                _tokenMap.CreateToken(TokenType.LeftParen),
                _tokenMap.CreateToken(TokenType.Identifier, "i"),
                _tokenMap.CreateToken(TokenType.LessThanOrEqual),
                _tokenMap.CreateToken(TokenType.Number, "10"),
                _tokenMap.CreateToken(TokenType.RightParen),
                _tokenMap.CreateToken(TokenType.Begin),
                _tokenMap.CreateToken(TokenType.Identifier, "print"),
                _tokenMap.CreateToken(TokenType.LeftParen),
                _tokenMap.CreateToken(TokenType.String, "i is less than or equal 10"),
                _tokenMap.CreateToken(TokenType.RightParen),
                _tokenMap.CreateToken(TokenType.End)
            };
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TokenizesWhileLoop()
        {
            var tokenStr = @"
                while (i<10)
                begin
                    i:=i+1
                    print(i)
                end
                ";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                _tokenMap.CreateToken(TokenType.While), 
                _tokenMap.CreateToken(TokenType.LeftParen),
                _tokenMap.CreateToken(TokenType.Identifier, "i"),
                _tokenMap.CreateToken(TokenType.LessThan),
                _tokenMap.CreateToken(TokenType.Number, "10"),
                _tokenMap.CreateToken(TokenType.RightParen),
                _tokenMap.CreateToken(TokenType.Begin),
                _tokenMap.CreateToken(TokenType.Identifier, "i"),
                _tokenMap.CreateToken(TokenType.Assign),
                _tokenMap.CreateToken(TokenType.Identifier, "i"),
                _tokenMap.CreateToken(TokenType.Plus),
                _tokenMap.CreateToken(TokenType.Number, "1"),
                _tokenMap.CreateToken(TokenType.Identifier, "print"),
                _tokenMap.CreateToken(TokenType.LeftParen),
                _tokenMap.CreateToken(TokenType.Identifier, "i"),
                _tokenMap.CreateToken(TokenType.RightParen),
                _tokenMap.CreateToken(TokenType.End)
            };
            
            Assert.Equal(expected, actual);
        }
    }
}