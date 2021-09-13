using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OneBot_UnitTest.Util
{
    public static class AssertUtil
    {
        public static void ArrayAreEqual<T, TU>(IEnumerable<T> expected, IEnumerable<TU> actual)
        {
            var expectedList = expected.ToList();
            var actualList = actual.ToList();
            
            Assert.AreEqual(expectedList.Count, actualList.Count);

            for (var i = 0; i < expectedList.Count; i++)
            {
                Assert.AreEqual(expectedList[i], actualList[i]);
            }
        }
    }
}
