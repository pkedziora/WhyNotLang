using System.Collections.Generic;
using WhyNotLang.Parser.Statements;

namespace WhyNotLang.Parser
{
    public interface IParser
    {
        List<IStatement> ParseAll(string program);
        void Initialise(string program);
        IStatement ParseNext();
    }
}