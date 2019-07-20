using WhyNotLang.Parser.Expressions;

namespace WhyNotLang.Parser
{
    public interface IExpressionParser
    {
        IExpression ParseExpression(string expression);
        IExpression ParseNextExpression();
    }
}