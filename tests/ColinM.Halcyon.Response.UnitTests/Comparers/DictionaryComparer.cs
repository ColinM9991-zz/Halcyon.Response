using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ColinM.Halcyon.Response.UnitTests.Comparers
{
    public class DictionaryComparer : IEqualityComparer<IReadOnlyDictionary<string, JToken>>
    {
        public bool Equals(IReadOnlyDictionary<string, JToken> x, IReadOnlyDictionary<string, JToken> y)
        {
            if (x.Count != y.Count)
                return false;
            if (x.Keys.Except(y.Keys).Any())
                return false;
            if (y.Keys.Except(x.Keys).Any())
                return false;
            foreach (var pair in x)
                if (!JToken.DeepEquals(pair.Value, y[pair.Key]))
                    return false;
            return true;
        }

        public int GetHashCode(IReadOnlyDictionary<string, JToken> obj)
            => obj.GetHashCode();
    }
}
