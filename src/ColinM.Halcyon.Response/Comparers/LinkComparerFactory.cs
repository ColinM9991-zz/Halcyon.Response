using Newtonsoft.Json.Linq;

namespace ColinM.Halcyon.Response.Comparers
{
    internal class LinkComparerFactory
    {
        private const string HrefKey = "href";
        internal LinkComparer CreateLinkComparer(JToken linkToken)
        {
            if (linkToken is JArray)
            {
                return new MultipleLinkComparer(HrefKey);
            }

            return new LinkComparer(HrefKey);
        }
    }
}
