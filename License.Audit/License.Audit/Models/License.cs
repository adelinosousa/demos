using System.Linq;
using System.Text.RegularExpressions;

namespace License.Audit
{
    public class License
    {
        private readonly Regex regularExpression;

        public string Name { get; }

        public string[] Keys { get; }

        public License(string name, params string[] keys)
        {
            Keys = keys;
            Name = name;

            regularExpression = new Regex($@"({string.Join("|", Keys.Select(key => @"\b" + key + @"\b"))})", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public License(string name, string key) : this(name, new string[] { key })
        {
        }

        public bool IsMatch(string context)
        {
            return regularExpression.IsMatch(context);
        }
    }
}
