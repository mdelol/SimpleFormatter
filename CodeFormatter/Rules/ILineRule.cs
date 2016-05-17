namespace CodeFormatter.Rules
{
    public interface ILineRule : IRule
    {
        bool Matches(string str);
        string Apply(string str);
    }
}