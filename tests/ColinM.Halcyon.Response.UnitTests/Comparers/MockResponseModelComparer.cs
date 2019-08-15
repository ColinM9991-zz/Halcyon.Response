using System;
using System.Collections.Generic;
using ColinM.Halcyon.Response.UnitTests.Models;

namespace ColinM.Halcyon.Response.UnitTests.Comparers
{
    public class MockResponseModelComparer : IEqualityComparer<MockResponseModel>
    {
        public bool Equals(MockResponseModel x, MockResponseModel y)
            => string.Equals(x.MockContent, y.MockContent, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode(MockResponseModel obj)
            => obj.GetHashCode();
    }
}
