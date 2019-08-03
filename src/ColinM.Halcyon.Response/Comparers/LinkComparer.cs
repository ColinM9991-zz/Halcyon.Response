using Newtonsoft.Json.Linq;
using System;

namespace ColinM.Halcyon.Response.Comparers
{
    public class LinkComparer
    {
        private readonly string hrefLinkKey;

        public LinkComparer(string hrefLinkKey)
        {
            this.hrefLinkKey = hrefLinkKey;
        }

        public virtual bool CompareLink(string expectedLinkValue, JToken link)
        {
            return string.Equals(expectedLinkValue, link[hrefLinkKey].Value<string>(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
