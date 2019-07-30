using System;
using System.Text;

namespace WhyNotLang.Tokenizer
{
    public class TokenReader : ITokenReader
    {
        public (int newIndex, int newLineCount) SkipWhitespace(string input, int index)
        {
            int newLineCount = 0;
            while (index < input.Length && char.IsWhiteSpace(input[index]))
            {
                if (input[index] == '\n')
                {
                    newLineCount++;
                }

                index++;
            }

            return (index, newLineCount);
        }
        
        public bool CanReadIdentifier(string input, int index)
        {
            return IsIdentifierChar(input[index]);
        }
        
        public (Token token, int endIndex) ReadIdentifier(string input, int index)
        {
            if (!char.IsLetter(input[index]) && input[index] != '_')
            {
                return (new Token(TokenType.Invalid, input[index].ToString()), index);
            }
            
            var sb = new StringBuilder();
            while (index < input.Length && IsIdentifierChar(input[index]))
            {
                sb.Append(input[index]);
                index++;
            }

            index--;

            var tokenStr = sb.ToString();

            return (new Token(TokenType.Identifier, tokenStr), index);
        }

        public bool CanReadNumber(string input, int index)
        {
            return char.IsDigit(input[index]);
        }
        
        public (Token token, int endIndex) ReadNumber(string input, int index)
        {
            var sb = new StringBuilder();
            while (index < input.Length && char.IsDigit(input[index]))
            {
                sb.Append(input[index]);
                index++;
            }

            index--;

            var tokenStr = sb.ToString();

            return (new Token(TokenType.Number, tokenStr), index);
        }
        
        public bool CanReadString(string input, int index)
        {
            return input[index] == '"';
        }
        
        public (Token token, int endIndex) ReadString(string input, int index)
        {
            if (input[index++] != '"')
            {
                throw new Exception("Invalid string");
            }
            var sb = new StringBuilder();
            while (index < input.Length && input[index] != '"')
            {
                sb.Append(input[index]);
                index++;
            }
            
            if (input[index] != '"')
            {
                throw new Exception("Invalid string");
            }

            var tokenStr = sb.ToString();
            
            return (new Token(TokenType.String, tokenStr), index);
        }
        
        private static bool IsIdentifierChar(char ch)
        {
            return char.IsDigit(ch) || char.IsLetter(ch) || ch == '_';
        }
    }
}