using System.Collections.Generic;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class WhileStatementTests
    {
        private Parser _parser;

        public WhileStatementTests()
        {
            _parser = TestHelpers.CreateParser();
        }

        [Fact]
        public void ParsesSimpleWhileStatement()
        {
            _parser.Initialise(@"
                while (x < y)
                    x := x + 1");

            var expectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var expectedBody = TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetBinaryExpressionWithIdentifiers("x", "+", 1));
            
            var expected = new WhileStatement(
               expectedCondition, 
               expectedBody);
            
            var actual =  _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesWhileStatementWithBlock()
        {
            _parser.Initialise(@"
                while (x < y)
                    begin
                        x := x + 1
                        y := y - 2
                    end");

            var expectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var expectedBody = new BlockStatement(new List<IStatement>()
            {
                TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetBinaryExpressionWithIdentifiers("x", "+", 1)),
                TestHelpers.GetVariableAssignementStatement("y", TestHelpers.GetBinaryExpressionWithIdentifiers("y", "-", 2))
            }); 
            
            var expected = new WhileStatement(
                expectedCondition, 
                expectedBody);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesWhileStatementWithComplexCondition()
        {
            _parser.Initialise(@"
                while (((1 == 2) and !(4 > 3)))
                    x := 1");
            
            var left = TestHelpers.GetBinaryExpression(1, "==", 2);
            var right = TestHelpers.GetUnaryExpression("!",TestHelpers.GetBinaryExpression(4, ">", 3));
            var expectedCondition = TestHelpers.GetBinaryExpression(left, "and", right);
            
            var expectedBody = TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1));
            
            var expected = new WhileStatement(
                expectedCondition, 
                expectedBody);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
    }
}