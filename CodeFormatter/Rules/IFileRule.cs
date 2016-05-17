using System.Collections.Generic;

namespace CodeFormatter.Rules
{
    public interface IFileRule : IRule
    {
        bool Matches(List<string> lines);
        List<string> Apply(List<string> lines);
    }
}