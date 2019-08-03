using Newtonsoft.Json.Linq;

namespace ColinM.Halcyon.Response.Comparers
{
    public class LinkComparerFactory
    {
        private const string HrefKey = "href";
        public LinkComparer CreateLinkComparer(JToken linkToken)
        {
            if (linkToken is JArray)
            {
                return new MultipleLinkComparer(HrefKey);
            }

            return new LinkComparer(HrefKey);
        }
    }
}
