using System.Collections.Generic;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Parser
{
    public class TokenIterator : ITokenIterator
    {
        public Token CurrentToken => _currentIndex < _tokens.Count ? _tokens[_currentIndex] : Token.Eof;
        private IList<Token> _tokens;
        private int _currentIndex;
        private readonly ITokenizer _tokenizer;

        public TokenIterator(ITokenizer tokenzer)
        {
            _tokenizer = tokenzer;
        }
        
        public void InitTokens(string str)
        {
            _tokens = _tokenizer.GetTokens(str);
            _currentIndex = 0;
        }
        
        public Token GetNextToken()
        {
            if (_currentIndex >= _tokens.Count - 1)
            {
                _currentIndex = _tokens.Count;
                return Token.Eof;
            }

            _currentIndex++;
            return _tokens[_currentIndex];
        }
        
        public Token PeekToken(int offset)
        {
            var peekIndex = _currentIndex + offset;
            if (peekIndex >= _tokens.Count)
            {
                return new Token(TokenType.Eof, "");
            }
            
            return _tokens[peekIndex];
        }
    }
}