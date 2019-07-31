using System;

namespace WhyNotLang.Tokenizer
{
    public class WhyNotLangException : Exception
    {
        public int LineNumber { get; set; }

        public WhyNotLangException(string message) : base(message)
        {
        }
        
        public WhyNotLangException(string message, int lineNumber, Exception innerException = null) 
            : base(message, innerException)
        {
            LineNumber = lineNumber;
        }
    }
}