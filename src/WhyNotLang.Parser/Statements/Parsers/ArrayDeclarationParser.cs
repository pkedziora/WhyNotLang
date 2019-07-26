using System;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements.Parsers
{
    public class ArrayDeclarationParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IExpressionParser _expressionParser;

        public ArrayDeclarationParser(ITokenIterator tokenIterator, IExpressionParser expressionParser)
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
            
            var arrayName = _tokenIterator.GetNextToken();
            
            //Parse brackets, should contain array size expression
            _tokenIterator.GetNextToken(); // Swallow arrayName
            if (_tokenIterator.CurrentToken.Type != TokenType.LeftBracket)
            {
                throw new ArgumentException("[ expected");
            }
            
            _tokenIterator.GetNextToken(); // Swallow [
            
            var arraySizeExpression =  _expressionParser.ParseNextExpression();
            
            if (_tokenIterator.CurrentToken.Type != TokenType.RightBracket)
            {
                throw new ArgumentException("] expected");
            }
            
            _tokenIterator.GetNextToken(); // Swallow ]

            var statement = new ArrayDeclarationStatement(arrayName, arraySizeExpression);

            return statement;
        }
    }
}