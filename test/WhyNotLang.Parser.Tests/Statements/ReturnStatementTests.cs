using System.Collections.Generic;
using WhyNotLang.Parser.Expressions;
using WhyNotLang.Parser.Statements;
using WhyNotLang.Tokenizer;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class ReturnStatementTests
    {
        private IParser _parser;
        public ReturnStatementTests()
        {
            _parser = TestHelpers.CreateParser();
        }
        
        [Fact]
        public void ParsesReturnStatementWith1Value()
        {
            _parser.Initialise("return 1");
            var expected = new ReturnStatement(
                TestHelpers.GetValueExpression(1));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesReturnStatementWith1Identifier()
        {
            _parser.Initialise("return abc");
            var expected = new ReturnStatement(
                TestHelpers.GetValueExpression("abc"));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesReturnStatementWithExpressionStartMinus()
        {
            _parser.Initialise("return -(1 + 2) - 3");
            
            var inner = TestHelpers.GetUnaryExpression("-", TestHelpers.GetBinaryExpression(1, "+", 2));
            var outer = TestHelpers.GetBinaryExpression(inner, "-", 3);
            
            var expected = new ReturnStatement(
                outer);
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesReturnStatementWithExpressionStartNot()
        {
            _parser.Initialise("return !a");
            
            var expected = new ReturnStatement(TestHelpers.GetUnaryExpression("!", "a"));
            
            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesReturnStatementAtEndOfFunction()
        {
            _parser.Initialise(@"
                function foo()
                begin
                    return 1
                end                
            ");
            var expected = new FunctionDeclarationStatement(
                new Token(TokenType.Identifier, "foo"), 
                new List<Token>(), 
                new BlockStatement(new List<IStatement>()
                {
                    new ReturnStatement(TestHelpers.GetValueExpression(1))
                }));
            
            var actual = _parser.ParseNext();

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesReturnStatementInMiddleOfFunction()
        {
            _parser.Initialise(@"
                function foo()
                begin
                    x := 1
                    return 1 + 2
                    y := 2
                end                
            ");
            var expected = new FunctionDeclarationStatement(
                new Token(TokenType.Identifier, "foo"), 
                new List<Token>(), 
                new BlockStatement(new List<IStatement>()
                {
                    new VariableAssignmentStatement(
                        new Token(TokenType.Identifier, "x"), 
                        new ValueExpression(new Token(TokenType.Number, "1"))),
                    new ReturnStatement(TestHelpers.GetBinaryExpression(1, "+", 2)),
                    new VariableAssignmentStatement(
                        new Token(TokenType.Identifier, "y"), 
                        new ValueExpression(new Token(TokenType.Number, "2")))
                }));
            
            var actual = _parser.ParseNext();

            Assert.Equal(expected, actual);
        }
    }
}