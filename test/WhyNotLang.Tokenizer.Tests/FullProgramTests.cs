using Xunit;

namespace WhyNotLang.Tokenizer.Tests
{
    public class FullProgramTests
    {
        private Tokenizer _tokenizer;
        private ITokenFactory _tokenFactory;
        
        public FullProgramTests()
        {
            _tokenFactory = new TokenFactory();
            _tokenizer = new Tokenizer(new TokenReader(), _tokenFactory);
        }
        
        [Fact]
        public void TokenizesHelloWorld()
        {
            var tokenStr = "print(\"Hello World\")";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                _tokenFactory.CreateToken(TokenType.Identifier, "print"), 
                _tokenFactory.CreateToken(TokenType.LeftParen),
                _tokenFactory.CreateToken(TokenType.String, "Hello World"),
                _tokenFactory.CreateToken(TokenType.RightParen),
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
                _tokenFactory.CreateToken(TokenType.Var), 
                _tokenFactory.CreateToken(TokenType.Identifier, "x"),
                _tokenFactory.CreateToken(TokenType.Assign),
                _tokenFactory.CreateToken(TokenType.Number, "1"),
                _tokenFactory.CreateToken(TokenType.Plus),
                _tokenFactory.CreateToken(TokenType.Number, "100")
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
                _tokenFactory.CreateToken(TokenType.Var), 
                _tokenFactory.CreateToken(TokenType.Identifier, "x"),
                _tokenFactory.CreateToken(TokenType.Assign),
                _tokenFactory.CreateToken(TokenType.Number, "2"),
                _tokenFactory.CreateToken(TokenType.Multiply),
                _tokenFactory.CreateToken(TokenType.LeftParen),
                _tokenFactory.CreateToken(TokenType.Number, "1"),
                _tokenFactory.CreateToken(TokenType.Plus),
                _tokenFactory.CreateToken(TokenType.LeftParen),
                _tokenFactory.CreateToken(TokenType.Number, "100"),
                _tokenFactory.CreateToken(TokenType.Divide),
                _tokenFactory.CreateToken(TokenType.Number, "2"),
                _tokenFactory.CreateToken(TokenType.RightParen),
                _tokenFactory.CreateToken(TokenType.RightParen),
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
                _tokenFactory.CreateToken(TokenType.If), 
                _tokenFactory.CreateToken(TokenType.LeftParen),
                _tokenFactory.CreateToken(TokenType.Identifier, "i"),
                _tokenFactory.CreateToken(TokenType.LessThanOrEqual),
                _tokenFactory.CreateToken(TokenType.Number, "10"),
                _tokenFactory.CreateToken(TokenType.RightParen),
                _tokenFactory.CreateToken(TokenType.Begin),
                _tokenFactory.CreateToken(TokenType.Identifier, "print"),
                _tokenFactory.CreateToken(TokenType.LeftParen),
                _tokenFactory.CreateToken(TokenType.String, "i is less than or equal 10"),
                _tokenFactory.CreateToken(TokenType.RightParen),
                _tokenFactory.CreateToken(TokenType.End)
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
                _tokenFactory.CreateToken(TokenType.While), 
                _tokenFactory.CreateToken(TokenType.LeftParen),
                _tokenFactory.CreateToken(TokenType.Identifier, "i"),
                _tokenFactory.CreateToken(TokenType.LessThan),
                _tokenFactory.CreateToken(TokenType.Number, "10"),
                _tokenFactory.CreateToken(TokenType.RightParen),
                _tokenFactory.CreateToken(TokenType.Begin),
                _tokenFactory.CreateToken(TokenType.Identifier, "i"),
                _tokenFactory.CreateToken(TokenType.Assign),
                _tokenFactory.CreateToken(TokenType.Identifier, "i"),
                _tokenFactory.CreateToken(TokenType.Plus),
                _tokenFactory.CreateToken(TokenType.Number, "1"),
                _tokenFactory.CreateToken(TokenType.Identifier, "print"),
                _tokenFactory.CreateToken(TokenType.LeftParen),
                _tokenFactory.CreateToken(TokenType.Identifier, "i"),
                _tokenFactory.CreateToken(TokenType.RightParen),
                _tokenFactory.CreateToken(TokenType.End)
            };
            
            Assert.Equal(expected, actual);
        }
    }
}