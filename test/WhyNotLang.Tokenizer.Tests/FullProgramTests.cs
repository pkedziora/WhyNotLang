using Xunit;

namespace WhyNotLang.Tokenizer.Tests
{
    public class FullProgramTests
    {
        private Tokenizer _tokenizer;
        public FullProgramTests()
        {
            _tokenizer = new Tokenizer(new TokenReader(), new TokenMap());
        }
        
        [Fact]
        public void TokenizesHelloWorld()
        {
            var tokenStr = "print(\"Hello World\")";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                new Token(TokenType.Identifier, "print"), 
                new Token(TokenType.LeftParen, "("),
                new Token(TokenType.String, "Hello World"),
                new Token(TokenType.RightParen, ")"),
            };
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TokenizesVariableAssignment()
        {
            var tokenStr = "var x = 1 + 100";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                new Token(TokenType.Var, "var"), 
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.Assign, "="),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Number, "100")
            };
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TokenizesMathExpression()
        {
            var tokenStr = "var x = 2 * (1 + (100 / 2))";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                new Token(TokenType.Var, "var"), 
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.Assign, "="),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Multiply, "*"),
                new Token(TokenType.LeftParen, "("),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.LeftParen, "("),
                new Token(TokenType.Number, "100"),
                new Token(TokenType.Divide, "/"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.RightParen, ")"),
                new Token(TokenType.RightParen, ")"),
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
                new Token(TokenType.If, "if"), 
                new Token(TokenType.LeftParen, "("),
                new Token(TokenType.Identifier, "i"),
                new Token(TokenType.LessThanOrEqual, "<="),
                new Token(TokenType.Number, "10"),
                new Token(TokenType.RightParen, ")"),
                new Token(TokenType.Begin, "begin"),
                new Token(TokenType.Identifier, "print"),
                new Token(TokenType.LeftParen, "("),
                new Token(TokenType.String, "i is less than or equal 10"),
                new Token(TokenType.RightParen, ")"),
                new Token(TokenType.End, "end")
            };
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TokenizesWhileLoop()
        {
            var tokenStr = @"
                while (i<10)
                begin
                    i=i+1
                    print(i)
                end
                ";
            var actual = _tokenizer.GetTokens(tokenStr);
            var expected =  new[]
            {
                new Token(TokenType.While, "while"), 
                new Token(TokenType.LeftParen, "("),
                new Token(TokenType.Identifier, "i"),
                new Token(TokenType.LessThan, "<"),
                new Token(TokenType.Number, "10"),
                new Token(TokenType.RightParen, ")"),
                new Token(TokenType.Begin, "begin"),
                new Token(TokenType.Identifier, "i"),
                new Token(TokenType.Assign, "="),
                new Token(TokenType.Identifier, "i"),
                new Token(TokenType.Plus, "+"),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Identifier, "print"),
                new Token(TokenType.LeftParen, "("),
                new Token(TokenType.Identifier, "i"),
                new Token(TokenType.RightParen, ")"),
                new Token(TokenType.End, "end")
            };
            
            Assert.Equal(expected, actual);
        }
    }
}