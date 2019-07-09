namespace WhyNotLang.Tokenizer
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var token = obj as Token;
            return Type == token.Type && Value == token.Value;
        }
    }
}