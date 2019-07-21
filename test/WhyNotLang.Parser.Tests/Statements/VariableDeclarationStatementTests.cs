using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class VariableDeclarationStatementTests
    {
        private IParser _parser;
        public VariableDeclarationStatementTests()
        {
            _parser = TestHelpers.CreateParser();
        }

        [Fact]
        public void ParsesDeclarationWithSingleNumber()
        {
            _parser.Initialise("var x := 1");
            var expected = new VariableDeclarationStatement(
                new Token(TokenType.Identifier, "x"), 
                new ValueExpression(new Token(TokenType.Number, "1")));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesDeclarationWith2PartExpression()
        {
            _parser.Initialise("var x := 1 + 2");
            var expected = new VariableDeclarationStatement(
                new Token(TokenType.Identifier, "x"), 
                TestHelpers.GetBinaryExpression(1, "+", 2));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesDeclarationWith2PartExpressionWithIdentifiers()
        {
            _parser.Initialise("var x := a + b");
            var expected = new VariableDeclarationStatement(
                new Token(TokenType.Identifier, "x"), 
                TestHelpers.GetBinaryExpressionWithIdentifiers("a", "+", "b"));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesDeclarationWith3PartExpression()
        {
            _parser.Initialise("var x := 1 + 2 * 3");
            var inner = TestHelpers.GetBinaryExpression(2, "*", 3);
            var expression = TestHelpers.GetBinaryExpression(1, "+", inner);
            var expected = new VariableDeclarationStatement(
                new Token(TokenType.Identifier, "x"), 
                expression);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesDeclarationWith3PartExpressionWithParens()
        {
            _parser.Initialise("var x := (1 + 2) * 3");
            var inner = TestHelpers.GetBinaryExpression(1, "+", 2);
            var expression = TestHelpers.GetBinaryExpression(inner, "*", 3);
            var expected = new VariableDeclarationStatement(
                new Token(TokenType.Identifier, "x"), 
                expression);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
    }
}