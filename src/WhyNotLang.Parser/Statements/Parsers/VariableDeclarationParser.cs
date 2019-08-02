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
            if (_tokenIterator.CurrentToken.Type != TokenType.Var && _tokenIterator.CurrentToken.Type != TokenType.Global)
            {
                throw new WhyNotLangException("var or global expected", _tokenIterator.CurrentToken.LineNumber);
            }

            var isGlobal = _tokenIterator.CurrentToken.Type == TokenType.Global;
            var variableName = _tokenIterator.GetNextToken();
            if (variableName.Type != TokenType.Identifier)
            {
                throw new WhyNotLangException("identifier expected", _tokenIterator.CurrentToken.LineNumber);    
            }
            
            _tokenIterator.GetNextToken();
            if (_tokenIterator.CurrentToken.Type != TokenType.Assign)
            {
                throw new WhyNotLangException(":= expected. Variables need to be initialised with value" , _tokenIterator.CurrentToken.LineNumber);
            }

            _tokenIterator.GetNextToken();
            var expression = _expressionParser.ParseNextExpression();
            var statement = new VariableDeclarationStatement(variableName, expression, isGlobal, variableName.LineNumber);

            return statement;
        }
    }
}