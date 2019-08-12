using System.Collections.Generic;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser.Statements.Parsers
{
    public class FunctionDeclarationParser : IStatementParser
    {
        private readonly ITokenIterator _tokenIterator;
        private readonly IParser _parser;

        public FunctionDeclarationParser(ITokenIterator tokenIterator, IParser parser)
        {
            _tokenIterator = tokenIterator;
            _parser = parser;
        }

        public IStatement Parse()
        {
            if (_tokenIterator.CurrentToken.Type != TokenType.Function)
            {
                throw new WhyNotLangException("function expected", _tokenIterator.CurrentToken.LineNumber);
            }

            _tokenIterator.GetNextToken(); // Set current to function name

            if (_tokenIterator.CurrentToken.Type != TokenType.Identifier)
            {
                throw new WhyNotLangException("identifier expected", _tokenIterator.CurrentToken.LineNumber);
            }

            var name = _tokenIterator.CurrentToken;

            _tokenIterator.GetNextToken(); // Set current to (
            var parameters = ParseParameters();

            if (_tokenIterator.CurrentToken.Type != TokenType.Begin)
            {
                throw new WhyNotLangException("begin expected", _tokenIterator.CurrentToken.LineNumber);
            }

            var body = (BlockStatement)_parser.ParseNext();

            return new FunctionDeclarationStatement(name, parameters, body, name.LineNumber);
        }

        private List<Token> ParseParameters()
        {
            if (_tokenIterator.CurrentToken.Type != TokenType.LeftParen)
            {
                throw new WhyNotLangException("( expected", _tokenIterator.CurrentToken.LineNumber);
            }

            _tokenIterator.GetNextToken(); // Swallow (
            var parameters = new List<Token>();
            while (_tokenIterator.CurrentToken.Type != TokenType.RightParen)
            {
                if (_tokenIterator.CurrentToken.Type != TokenType.Identifier)
                {
                    throw new WhyNotLangException("identifier expected", _tokenIterator.CurrentToken.LineNumber);
                }

                parameters.Add(_tokenIterator.CurrentToken);
                _tokenIterator.GetNextToken();
                if (_tokenIterator.CurrentToken.Type == TokenType.Comma)
                {
                    _tokenIterator.GetNextToken(); // Swallow ,
                }
            }

            _tokenIterator.GetNextToken(); // Swallow )

            return parameters;
        }
    }
}