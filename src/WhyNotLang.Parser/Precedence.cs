namespace WhyNotLang.Parser
{
    public enum Precedence
    {
        None,
        Or,
        And,
        Comparison,
        AddSub,
        MulDiv,
    }
}