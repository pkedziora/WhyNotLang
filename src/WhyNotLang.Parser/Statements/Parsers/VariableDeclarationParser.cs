using System;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements.Parsers
{
    public class VariableDeclarationParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;

        public VariableDeclarationParser(ITokenIterator tokenIterator, IExpressionParser expressionParser)
        {
            _tokenIterator = tokenIterator;
            _expressionParser = expressionParser;
        }
        
        public IStatement Parse()
        {
            if (_tokenIterator.CurrentToken.Type != TokenType.Var)
            {
                throw new ArgumentException("var expected");
            }
            
            var variableName = _tokenIterator.GetNextToken();
            _tokenIterator.GetNextToken();
            if (_tokenIterator.CurrentToken.Type != TokenType.Assign)
            {
                throw new ArgumentException(":= expected. Variables need to be initialised with value");
            }

            _tokenIterator.GetNextToken();
            var expression = _expressionParser.ParseNextExpression();
            var statement = new VariableDeclarationStatement(variableName, expression);

            return statement;
        }
    }
}