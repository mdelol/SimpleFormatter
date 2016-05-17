using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

namespace CodeFormatter.Rules.LineRules
{
    [Export(typeof(IRule))]
    public class CommaRule : ILineRule
    {
        private readonly Regex _noWhitespaceRegex = new Regex(@"([\s\S]*?),\s{0}([\S].*)");
        private readonly Regex _tooManyWhitespacesRegex = new Regex(@"([\w]*),\s\s+(.*)");
        private readonly Regex _whitespaceBeforeCommaRegex = new Regex(@"([\s\S]*?)\s+,([\s\S]*)"); //@TODO mb i can use single regex? 
        //@TODO also, it shouldn't catch strings here, but it does. 


        public IEnumerable<string> ApplicableTo => new List<string> { "java", "cs" };

        public bool Matches(string str)
        {
            return _noWhitespaceRegex.IsMatch(str) || _tooManyWhitespacesRegex.IsMatch(str) || _whitespaceBeforeCommaRegex.IsMatch(str);
        }

        public string Apply(string str)
        {
            while (_noWhitespaceRegex.IsMatch(str))
            {
                var match = _noWhitespaceRegex.Match(str);
                var valueLeft = match.Groups[1].Value;
                var valueRight = match.Groups[2].Value;
                str = $"{valueLeft}, {valueRight}";
            }

            while (_tooManyWhitespacesRegex.IsMatch(str))
            {
                var match = _tooManyWhitespacesRegex.Match(str);
                var valueLeft = match.Groups[1].Value;
                var valueRight = match.Groups[2].Value;
                str = $"{valueLeft}, {valueRight}";
            }

            while (_whitespaceBeforeCommaRegex.IsMatch(str))
            {
                var match = _whitespaceBeforeCommaRegex.Match(str);
                var valueLeft = match.Groups[1].Value;
                var valueRight = match.Groups[2].Value;
                str = $"{valueLeft}, {valueRight}";
            }

            return str;
        }
    }
}