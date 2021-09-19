using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.CommandRoute.Utils;
using Sora.Entities.MessageElement;

namespace OneBot_UnitTest
{
    [TestClass]
    public class CQCodeUtilTest
    {
        [TestMethod]
        public void CQCodeEncodeTest()
        {
            Assert.AreEqual("&#91;", "[".CQCodeEncode());
            Assert.AreEqual("&#93;", "]".CQCodeEncode());
            Assert.AreEqual(",", ",".CQCodeEncode());
            Assert.AreEqual("&amp;", "&".CQCodeEncode());

            Assert.AreEqual("&#91;", "[".CQCodeEncode(true));
            Assert.AreEqual("&#93;", "]".CQCodeEncode(true));
            Assert.AreEqual("&#44;", ",".CQCodeEncode(true));
            Assert.AreEqual("&amp;", "&".CQCodeEncode(true));

            Assert.AreEqual("&#91;CQ:at,qq=114514&#93;", "[CQ:at,qq=114514]".CQCodeEncode());
            Assert.AreEqual("&#91;CQ:at&#44;qq=114514&#93;", "[CQ:at,qq=114514]".CQCodeEncode(true));
        }

        [TestMethod]
        public void CQCodeDecodeTest()
        {
            Assert.AreEqual("[", "&#91;".CQCodeDecode());
            Assert.AreEqual("]", "&#93;".CQCodeDecode());
            Assert.AreEqual(",", ",".CQCodeDecode());
            Assert.AreEqual("&", "&amp;".CQCodeDecode());

            Assert.AreEqual("[", "&#91;".CQCodeDecode());
            Assert.AreEqual("]", "&#93;".CQCodeDecode());
            Assert.AreEqual(",", "&#44;".CQCodeDecode());
            Assert.AreEqual("&", "&amp;".CQCodeDecode());

            Assert.AreEqual("[CQ:at,qq=114514]", "&#91;CQ:at,qq=114514&#93;".CQCodeDecode());
            Assert.AreEqual("810[CQ:at,qq=114514]1919", "810&#91;CQ:at,qq=114514&#93;1919".CQCodeDecode());
            Assert.AreEqual("[CQ:at,qq=114514]", "&#91;CQ:at&#44;qq=114514&#93;".CQCodeDecode());
            Assert.AreEqual("[CQ:at,qq=114514]1919810", "&#91;CQ:at&#44;qq=114514&#93;1919810".CQCodeDecode());

            var test = Guid.NewGuid().ToString();
            Assert.AreEqual(test, test.CQCodeDecode());
            
            Assert.AreEqual("&", "&".CQCodeDecode());
            Assert.AreEqual("&123;", "&123;".CQCodeDecode());
            Assert.AreEqual("&123;[CQ:at,qq=114514]", "&123;&#91;CQ:at&#44;qq=114514&#93;".CQCodeDecode());
        }

        [TestMethod]
        public void CQCodeSerializeTest()
        {
            Assert.AreEqual("[CQ:at,qq=114514]", CQCodes.CQAt(114514).Serialize());
            Assert.AreEqual("[CQ:at,qq=1919810]", CQCodes.CQAt(1919810).Serialize());
        }

        [TestMethod]
        public void MessageBodySerializeTest()
        {
            Assert.AreEqual("[CQ:at,qq=114514]1919810", (CQCodes.CQAt(114514) + "1919810").Serialize());
            Assert.AreEqual("[CQ:at,qq=1919810]114514", (CQCodes.CQAt(1919810) + "114514").Serialize());
            Assert.AreEqual("1919810[CQ:at,qq=1919810]114514", ("1919810" + CQCodes.CQAt(1919810) + "114514").Serialize());
        }


    }
}
