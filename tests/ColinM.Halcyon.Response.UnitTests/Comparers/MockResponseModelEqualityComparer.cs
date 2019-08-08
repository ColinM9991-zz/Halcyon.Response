using System;
using System.Collections.Generic;

namespace ColinM.Halcyon.Response.UnitTests.Comparers
{
    public class MockResponseModelEqualityComparer : IEqualityComparer<MockResponseModel>
    {
        public bool Equals(MockResponseModel x, MockResponseModel y)
            => string.Equals(x.MockContent, y.MockContent, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode(MockResponseModel obj)
            => obj.GetHashCode();
    }
}
