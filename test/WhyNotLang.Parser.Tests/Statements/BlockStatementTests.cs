using System.Collections.Generic;
using WhyNotLang.Parser.Statements;
using Xunit;

namespace WhyNotLang.Parser.Tests.Statements
{
    public class BlockStatementTests
    {
        private Parser _parser;

        public BlockStatementTests()
        {
            _parser = TestHelpers.CreateParser();
        }
        
        [Fact]
        public void ParsesEmptyBlockStatement()
        {
            _parser.Initialise(@"
                begin
                end");
            
            var expected = new BlockStatement(new List<IStatement>());

            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesBlockStatementWith1Statement()
        {
            _parser.Initialise(@"
                begin
                    x := 1
                end");
            
            var expected = new BlockStatement(new List<IStatement>
            {
                TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1))
            });

            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesBlockStatementWith2Statements()
        {
            _parser.Initialise(@"
                begin
                    x := 1
                    var y := 2
                end");
            
            var expected = new BlockStatement(new List<IStatement>
            {
                TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1)),
                TestHelpers.GetVariableDeclarationStatement("y", TestHelpers.GetValueExpression(2))
            });

            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesBlockStatementWith3Statements()
        {
            _parser.Initialise(@"
                begin
                    x := 1
                    var y := 2
                    var abc := (2 + 2) * 3
                end");
            
            var expected = new BlockStatement(new List<IStatement>
            {
                TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1)),
                TestHelpers.GetVariableDeclarationStatement("y", TestHelpers.GetValueExpression(2)),
                TestHelpers.GetVariableDeclarationStatement("abc", TestHelpers.GetBinaryExpression(
                    TestHelpers.GetBinaryExpression(2, "+", 2), "*", 3))
            });

            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void ParsesNestedBlockStatementWith3Statements()
        {
            _parser.Initialise(@"
                begin
                    x := 1
                    var y := 2
                    begin
                        inner1 := 100
                        var inner2 := (200 + 200) * 300
                    end
                    var abc := (2 + 2) * 3
                end");
            
            var expected = new BlockStatement(new List<IStatement>
            {
                TestHelpers.GetVariableAssignementStatement("x", TestHelpers.GetValueExpression(1)),
                TestHelpers.GetVariableDeclarationStatement("y", TestHelpers.GetValueExpression(2)),
                new BlockStatement(new List<IStatement>()
                {
                    TestHelpers.GetVariableAssignementStatement("inner1", TestHelpers.GetValueExpression(100)),
                    TestHelpers.GetVariableDeclarationStatement("inner2", TestHelpers.GetBinaryExpression(
                        TestHelpers.GetBinaryExpression(200, "+", 200), "*", 300))
                }),
                TestHelpers.GetVariableDeclarationStatement("abc", TestHelpers.GetBinaryExpression(
                    TestHelpers.GetBinaryExpression(2, "+", 2), "*", 3))
            });

            var actual = _parser.ParseNext();
            
            Assert.Equal(expected, actual);
        }
    }
}