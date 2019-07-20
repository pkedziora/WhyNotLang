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

            var expectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var expectedBody = TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1));
            
            var expected = new IfStatement(
               expectedCondition, 
               expectedBody);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesIfStatementWithComplexCondition()
        {
            _parser.Initialise(@"
                if (((1 == 2) and !(4 > 3)))
                    x = 1");
            
            var left = TestHelpers.GetBinaryExpression(1, "==", 2);
            var right = TestHelpers.GetUnaryExpression("!",TestHelpers.GetBinaryExpression(4, ">", 3));
            var expectedCondition = TestHelpers.GetBinaryExpression(left, "and", right);
            
            var expectedBody = TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1));
            
            var expected = new IfStatement(
                expectedCondition, 
                expectedBody);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesIfStatementWithComplexConditionAndComplexBody()
        {
            _parser.Initialise(@"
                if (((1 == 2) and !(4 > 3)))
                    var x = (1 + 2) * 3");
            
            var left = TestHelpers.GetBinaryExpression(1, "==", 2);
            var right = TestHelpers.GetUnaryExpression("!",TestHelpers.GetBinaryExpression(4, ">", 3));
            var expectedCondition = TestHelpers.GetBinaryExpression(left, "and", right);
            
            var inner = TestHelpers.GetBinaryExpression(1, "+", 2);
            var expression = TestHelpers.GetBinaryExpression(inner, "*", 3);
            var expectedBody = new VariableDeclarationStatement(
                new Token(TokenType.Identifier, "x"), 
                expression);

            var expected = new IfStatement(
                expectedCondition, 
                expectedBody);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesSimpleIfElseStatement()
        {
            _parser.Initialise(@"
                if (x < y)
                    x = 1
                else
                    y = 2");

            var expectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var expectedBody = TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1));
            var expectedElse = TestHelpers.GetVariableAssignementStatement("y", TestHelpers.GetValueExpression(2));
            
            var expected = new IfStatement(
                expectedCondition, 
                expectedBody,
                expectedElse);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
    }
}