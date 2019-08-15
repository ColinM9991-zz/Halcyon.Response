using System.Collections.Generic;
using ColinM.Halcyon.Response.UnitTests.Models;

namespace ColinM.Halcyon.Response.UnitTests.Comparers
{
    public class HalcyonResponseContactResourceComparer : IEqualityComparer<HalcyonResponseModel<ContactResource>>
    {
        private readonly DictionaryComparer dictionaryEqualityComparer = new DictionaryComparer();

        public bool Equals(HalcyonResponseModel<ContactResource> x, HalcyonResponseModel<ContactResource> y)
            => x.Model.Id.Equals(y.Model.Id)
                && x.Model.Name.Equals(y.Model.Name)
                && dictionaryEqualityComparer.Equals(x.Links, y.Links)
                && dictionaryEqualityComparer.Equals(x.Embedded, y.Embedded);

        public int GetHashCode(HalcyonResponseModel<ContactResource> obj)
            => obj.GetHashCode();
    }
}
