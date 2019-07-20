using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class IfStatementTests
    {
        private Parser _parser;

        public IfStatementTests()
        {
            _parser = TestHelpers.CreateParser();
        }

        [Fact]
        public void ParsesSimpleIfStatement()
        {
            _parser.Initialise(@"
                if (x < y)
                    x = 1");

            var expectedTestExpression = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var expectedBody = new VariableAssignmentStatement(new Token(TokenType.Identifier, "x"),
                new ValueExpression(new Token(TokenType.Number, "1")));
            
            var expected = new IfStatement(
               expectedTestExpression, 
               expectedBody);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
    }
}