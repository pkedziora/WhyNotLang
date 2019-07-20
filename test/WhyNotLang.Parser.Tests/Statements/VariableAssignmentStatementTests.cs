using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class VariableAssignmentStatementTests
    {
        private IParser _parser;
        public VariableAssignmentStatementTests()
        {
            _parser = TestHelpers.CreateParser();
        }

        [Fact]
        public void ParsesAssignmentWithSingleNumber()
        {
            _parser.Initialise("x = 1");
            var expected = new VariableAssignmentStatement(
                new Token(TokenType.Identifier, "x"), 
                new ValueExpression(new Token(TokenType.Number, "1")));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
    }
}