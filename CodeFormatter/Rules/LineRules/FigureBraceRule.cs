using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

namespace CodeFormatter.Rules.LineRules
{
    [Export(typeof(IRule))]
    public class FigureBraceRule : ILineRule
    {
        private readonly Regex _regex = new Regex(@"([\S\s]*[\S]+){[\s]*\Z");

        public IEnumerable<string> ApplicableTo => new List<string> { "java" };

        public bool Matches(string str)
        {
            return _regex.IsMatch(str);
        }

        public string Apply(string str)
        {
            var match = _regex.Match(str);
            var value = match.Groups[1].Value;
            return $"{value} {{";
        }
    }
}