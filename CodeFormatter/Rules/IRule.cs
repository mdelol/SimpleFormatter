
using System.Collections.Generic;

namespace CodeFormatter.Rules
{
    public interface IRule
    {
        IEnumerable<string> ApplicableTo { get; }
    }
}