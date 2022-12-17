using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using OneBot.Core.Util;

namespace OneBot.UnitTest;

[TestClass]
public class OntBotModelTest
{
    public class TestTextSegment : Text
    {
        public TestTextSegment(string text, string anotherField)
        {
            Text = text;
            AnotherField = anotherField;
        }

        public string Text { get; }

        public string AnotherField { get; }

        public static MessageSegmentRef Build(string msg, string anotherField)
        {
            return new MessageSegment<Text>("text", new TestTextSegment(msg, anotherField));
        }
    }


    [TestMethod]
    public void TestGenericGet()
    {
        MessageSegmentRef seg = TestTextSegment.Build("Test", "anotherField");
        Assert.AreEqual("anotherField", seg.Get<string>("AnotherField"));
    }
}