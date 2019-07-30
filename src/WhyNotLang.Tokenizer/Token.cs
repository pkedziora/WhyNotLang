namespace WhyNotLang.Tokenizer
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public int LineNumber { get; set; }

        public static Token Eof => new Token(TokenType.Eof, "");
        
        public Token(TokenType type, string value, int lineNumber = 0)
        {
            Type = type;
            Value = value;
            LineNumber = lineNumber;
        }

        public override bool Equals(object obj)
        {
            var token = obj as Token;
            if (token == null)
            {
                return false;
            }
            
            return Type == token.Type && Value == token.Value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Type.GetHashCode();
                hash = hash * 23 + Value.GetHashCode();

                return hash;
            }
        }
    }
}