using Newtonsoft.Json.Linq;

namespace ColinM.Halcyon.Response.Comparers
{
    public class MultipleLinkComparer : LinkComparer
    {
        public MultipleLinkComparer(string hrefLinkKey)
            : base(hrefLinkKey)
        {
        }

        public override bool CompareLink(string expectedLinkValue, JToken links)
        {
            var arrayOfLinks = (JArray)links;
            foreach (var link in arrayOfLinks)
            {
                if (base.CompareLink(expectedLinkValue, link))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
