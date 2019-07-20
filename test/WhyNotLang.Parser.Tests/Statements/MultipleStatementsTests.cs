using System.Collections.Generic;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class MultipleStatementsTests
    {
        private Parser _parser;

        public MultipleStatementsTests()
        {
            _parser = TestHelpers.CreateParser();
        }

        [Fact]
        public void Parses2AssignmentsWithSingleNumber()
        {
            _parser.Initialise(
                @" x = 1
                            y = 2");
            var expected = new List<IStatement>
            {
                new VariableAssignmentStatement(
                    new Token(TokenType.Identifier, "x"),
                    new ValueExpression(new Token(TokenType.Number, "1"))),
                new VariableAssignmentStatement(
                    new Token(TokenType.Identifier, "y"),
                    new ValueExpression(new Token(TokenType.Number, "2")))
            };

            var actual = _parser.ParseAll();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2AssignmentsWithSimpleExpressions()
        {
            _parser.Initialise(
                @" x = 1 + 2
                            y = 1 * 2");
            var expected = new List<IStatement>
            {
                new VariableAssignmentStatement(
                    new Token(TokenType.Identifier, "x"),
                    TestHelpers.GetBinaryExpression(1, "+", 2)),
                new VariableAssignmentStatement(
                    new Token(TokenType.Identifier, "y"),
                    TestHelpers.GetBinaryExpression(1, "*", 2))
            };

            var actual = _parser.ParseAll();
            
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Parses1DeclarationAnd1AssignmentsWithSimpleExpressions()
        {
            _parser.Initialise(
                @" var x = 1 + 2
                            y = 1 * 2");
            var expected = new List<IStatement>
            {
                new VariableDeclarationStatement(
                    new Token(TokenType.Identifier, "x"),
                    TestHelpers.GetBinaryExpression(1, "+", 2)),
                new VariableAssignmentStatement(
                    new Token(TokenType.Identifier, "y"),
                    TestHelpers.GetBinaryExpression(1, "*", 2))
            };

            var actual = _parser.ParseAll();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2AssignmentsWithComplexExpressions()
        {
            _parser.Initialise(
                @" x = (1 + 2) * 3
                            y = (1 * (2 + 3))");
            var expected = new List<IStatement>
            {
                new VariableAssignmentStatement(
                    new Token(TokenType.Identifier, "x"),
                    TestHelpers.GetBinaryExpression(TestHelpers.GetBinaryExpression(1, "+", 2), "*", 3)),
                new VariableAssignmentStatement(
                    new Token(TokenType.Identifier, "y"),
                    TestHelpers.GetBinaryExpression(1, "*", TestHelpers.GetBinaryExpression(2, "+", 3)))
            };

            var actual = _parser.ParseAll();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1DeclarationAnd1AssignmentsWithComplexExpressions()
        {
            _parser.Initialise(
                @" x = (1 + 2) * 3
                            var y = (1 * (2 + 3))");
            var expected = new List<IStatement>
            {
                new VariableAssignmentStatement(
                    new Token(TokenType.Identifier, "x"),
                    TestHelpers.GetBinaryExpression(TestHelpers.GetBinaryExpression(1, "+", 2), "*", 3)),
                new VariableDeclarationStatement(
                    new Token(TokenType.Identifier, "y"),
                    TestHelpers.GetBinaryExpression(1, "*", TestHelpers.GetBinaryExpression(2, "+", 3)))
            };

            var actual = _parser.ParseAll();
            
            Assert.Equal(expected, actual);
        }
    }
}