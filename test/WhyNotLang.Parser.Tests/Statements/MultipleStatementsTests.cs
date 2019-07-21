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
                @" x := 1
                            y := 2");
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
                @" x := 1 + 2
                            y := 1 * 2");
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
                @" var x := 1 + 2
                            y := 1 * 2");
            var expected = new List<IStatement>
            {
                TestHelpers.GetVariableDeclarationStatement("x",
                    TestHelpers.GetBinaryExpression(1, "+", 2)),
                TestHelpers.GetVariableAssignementStatement( "y",
                    TestHelpers.GetBinaryExpression(1, "*", 2))
            };

            var actual = _parser.ParseAll();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses2AssignmentsWithComplexExpressions()
        {
            _parser.Initialise(
                @" x := (1 + 2) * 3
                            y := (1 * (2 + 3))");
            var expected = new List<IStatement>
            {
                TestHelpers.GetVariableAssignementStatement("x", 
                    TestHelpers.GetBinaryExpression(TestHelpers.GetBinaryExpression(1, "+", 2), "*", 3)),
                TestHelpers.GetVariableAssignementStatement( "y",
                    TestHelpers.GetBinaryExpression(1, "*", TestHelpers.GetBinaryExpression(2, "+", 3)))
            };

            var actual = _parser.ParseAll();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Parses1DeclarationAnd1AssignmentsWithComplexExpressions()
        {
            _parser.Initialise(
                @" x := (1 + 2) * 3
                            var y := (1 * (2 + 3))");
            var expected = new List<IStatement>
            {
                TestHelpers.GetVariableAssignementStatement("x",
                    TestHelpers.GetBinaryExpression(TestHelpers.GetBinaryExpression(1, "+", 2), "*", 3)),
                TestHelpers.GetVariableDeclarationStatement("y",
                    TestHelpers.GetBinaryExpression(1, "*", TestHelpers.GetBinaryExpression(2, "+", 3)))
            };

            var actual = _parser.ParseAll();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesVarAssignmentAndIfAndWhileAndFunctionAndVarDeclaration()
        {
            _parser.Initialise(
                @" x := (1 + 2) * 3
                            if (x < y)
                                x := 1
                            else
                                y := 2
                            while (x < y)
                            begin
                                x := x + 1
                                y := y - 2
                            end
                            foo(1,2)
                            var y := (1 * (2 + 3))");

            //If
            var expectedIfCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var expectedIfBody = TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1));
            var expectedElse = TestHelpers.GetVariableAssignementStatement("y", TestHelpers.GetValueExpression(2));
            var expectedIfStatement = new IfStatement(
                expectedIfCondition, 
                expectedIfBody,
                expectedElse);
            
            //While
            var expectedCondition = TestHelpers.GetBinaryExpressionWithIdentifiers("x", "<", "y");
            var expectedBody = new BlockStatement(new List<IStatement>()
            {
                TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetBinaryExpressionWithIdentifiers("x", "+", 1)),
                TestHelpers.GetVariableAssignementStatement("y", TestHelpers.GetBinaryExpressionWithIdentifiers("y", "-", 2))
            });
            var expectedWhileStatement = new WhileStatement(
                expectedCondition, 
                expectedBody);
            
            //Function call
            var expectedParameters = new List<IExpression>
            {
                new ValueExpression(new Token(TokenType.Number, "1")),
                new ValueExpression(new Token(TokenType.Number, "2")),
            };
            var expectedFunctionCallStatement = new FunctionCallStatement(new FunctionExpression(new Token(TokenType.Identifier, "foo"), 
                expectedParameters));
            
            var expected = new List<IStatement>
            {
                TestHelpers.GetVariableAssignementStatement("x",
                    TestHelpers.GetBinaryExpression(TestHelpers.GetBinaryExpression(1, "+", 2), "*", 3)),
                expectedIfStatement,
                expectedWhileStatement,
                expectedFunctionCallStatement,
                TestHelpers.GetVariableDeclarationStatement("y",
                    TestHelpers.GetBinaryExpression(1, "*", TestHelpers.GetBinaryExpression(2, "+", 3)))
            };

            var actual = _parser.ParseAll();
            
            Assert.Equal(expected, actual);
        }
    }
}