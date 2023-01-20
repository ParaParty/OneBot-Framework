using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.Core.Extension;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;

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

        public static IMessageSegment<Text> Build(string msg, string anotherField)
        {
            return new MessageSegment<Text>(new TestTextSegment(msg, anotherField));
        }
    }

    [TestMethod]
    public void TestGenericGet()
    {
        IMessageSegment<Text> seg = TestTextSegment.Build("Test", "anotherField");
        Assert.AreEqual("text", seg.Type);
        Assert.AreEqual("anotherField", seg.Get<string>("AnotherField"));
    }

    [TestMethod]
    public void TestDictionaryGet()
    {
        var data = new DictionaryMessageSegment
        {
            ["field_a"] = "1",
            ["field_b"] = 2
        };
        var t = new MessageSegment<DictionaryMessageSegment>("custom_type", data);
        Assert.AreEqual("custom_type", t.Type);
        Assert.AreEqual("1", t.Get<string>("field_a"));
        Assert.AreEqual(2, t.Get<int>("field_b"));
    }
}
