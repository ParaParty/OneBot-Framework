using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OneBot_UnitTest.Util
{
    public static class AssertUtil
    {
        public static void ArrayAreEqual(IEnumerable expected, IEnumerable actual)
        {
            var expectedList = expected.Cast<object>().ToList();
            var actualList = actual.Cast<object>().ToList();

            Assert.AreEqual(expectedList.Count, actualList.Count);

            for (var i = 0; i < expectedList.Count; i++)
            {
                if (
                    expectedList[i] is not string && actualList[i] is not string &&
                    expectedList[i] is IEnumerable && actualList[i] is IEnumerable
                )
                {
                    ArrayAreEqual(expectedList[i] as IEnumerable, actualList[i] as IEnumerable);
                }
                else
                {
                    Assert.AreEqual(expectedList[i], actualList[i]);
                }
            }
        }
    }
}
