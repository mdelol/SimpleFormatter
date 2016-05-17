using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CodeFormatter.Rules.FileRules
{
    [Export(typeof(IRule))]
    public class EmptyLineRule : IFileRule
    {
        public bool Matches(List<string> lines)
        {
            return lines.Any() && string.IsNullOrEmpty(lines.Last());
        }

        public List<string> Apply(List<string> lines)
        {
            if (string.IsNullOrEmpty(lines.Last()))
            {
                var result = new List<string>(lines);
                result.Reverse();
                var skipWhile = result.SkipWhile(string.IsNullOrEmpty);
                var enumerable = skipWhile.Reverse();
                var list = enumerable.ToList();

                return list;
            }
            return lines;
        }

        public IEnumerable<string> ApplicableTo => new List<string> { "java", "cs", "proto" };
    }
}
