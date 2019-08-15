using ColinM.Halcyon.Response.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColinM.Halcyon.Response.UnitTests.Comparers
{
    public class MockResponseModelArrayComparer : IEqualityComparer<HalcyonResponseModel<MockResponseModel>[]>
    {
        private readonly MockResponseModelComparer mockResponseModelComparer = new MockResponseModelComparer();
        private readonly DictionaryComparer dictionaryComparer = new DictionaryComparer();

        public bool Equals(HalcyonResponseModel<MockResponseModel>[] x, HalcyonResponseModel<MockResponseModel>[] y)
        {
            foreach (var firstResponseModel in x)
            {
                var secondResponseModel = y.FirstOrDefault(responseModel => string.Equals(responseModel.Model.MockContent, firstResponseModel.Model.MockContent));
                if (secondResponseModel == null)
                    return false;

                if (!mockResponseModelComparer.Equals(firstResponseModel.Model, secondResponseModel.Model))
                    return false;

                if (!dictionaryComparer.Equals(firstResponseModel.Links, secondResponseModel.Links) || !dictionaryComparer.Equals(firstResponseModel.Embedded, secondResponseModel.Embedded))
                    return false;
            }

            return true;
        }

        public int GetHashCode(HalcyonResponseModel<MockResponseModel>[] obj)
        {
            throw new NotImplementedException();
        }
    }
}
