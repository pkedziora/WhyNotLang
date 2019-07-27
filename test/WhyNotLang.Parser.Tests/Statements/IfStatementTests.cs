using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using WhyNotLang.Interpreter;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Test.Common;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class IfStatementTests
    {
        private IParser _parser;

        public IfStatementTests()
        {
            var serviceProvider = IoC.BuildServiceProvider();
            _parser = serviceProvider.GetService<IParser>();
        }

        [Fact]
        public void ParsesSimpleIfStatement()
        {
            _parser.Initialise(@"
                if (x < y)
                    x := 1");

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
                    x := 1");
            
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
                    var x := (1 + 2) * 3");
            
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
                    x := 1
                else
                    y := 2");

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
        
        [Fact]
        public void ParsesIfElseIfElseStatement()
        {
            _parser.Initialise(@"
                if (x < y)
                    x := 1
                else if (y == z)
                    y := 2 * 3
                else
                    z := (4)");

            var firstExpectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var firstExpectedBody = TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1));
            
            var secondExpectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("y", "==", "z");
            var secondExpectedBody = TestHelpers.GetVariableAssignementStatement("y", TestHelpers.GetBinaryExpression(2, "*", 3));
            
            var thirdExpectedBody = TestHelpers.GetVariableAssignementStatement("z", TestHelpers.GetValueExpression(4));
            
            var expected = new IfStatement(
                firstExpectedCondition, 
                firstExpectedBody,
                new IfStatement(secondExpectedCondition, secondExpectedBody, thirdExpectedBody));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesIfElseIfElseStatementWithBlock()
        {
            _parser.Initialise(@"
                if (x < y)
                    x := 1
                else if (y == z)
                begin
                    y := 2 * 3
                    ab := a * b
                end
                else
                    z := (4)");

            var firstExpectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var firstExpectedBody = TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1));
            
            var secondExpectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("y", "==", "z");
            var secondExpectedBody = 
                new BlockStatement(new List<IStatement>()
                {
                    TestHelpers.GetVariableAssignementStatement("y", TestHelpers.GetBinaryExpression(2, "*", 3)),
                    TestHelpers.GetVariableAssignementStatement("ab", TestHelpers.GetBinaryExpressionWithIdentifiers("a", "*", "b"))
                });

            var thirdExpectedBody = TestHelpers.GetVariableAssignementStatement("z", TestHelpers.GetValueExpression(4));
            
            var expected = new IfStatement(
                firstExpectedCondition, 
                firstExpectedBody,
                new IfStatement(secondExpectedCondition, secondExpectedBody, thirdExpectedBody));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesIfStatementWithBlock()
        {
            _parser.Initialise(@"
                if (x < y)
                    begin
                        x := 1
                        z := 2
                    end");

            var expectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var expectedBody = new BlockStatement(new List<IStatement>()
            {
                TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1)),
                TestHelpers.GetVariableAssignementStatement("z", TestHelpers.GetValueExpression(2))
            }); 
            
            var expected = new IfStatement(
                expectedCondition, 
                expectedBody);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
    }
}