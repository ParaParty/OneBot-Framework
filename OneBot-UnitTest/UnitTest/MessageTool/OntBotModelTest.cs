using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.MessageSegmentData;
using OneBot.Core.Model.Message.SimpleMessageSegment;
using OneBot.Core.Util;

namespace OneBot.UnitTest.MessageTool;

[TestClass]
public class OntBotModelTest
{
    public class TestTextSegment : Text
    {
        public TestTextSegment(string text, string anotherField) : base(text)
        {
            Add("another_field", anotherField);
        }


        public static IMessageSegment Build(string msg, string anotherField)
        {
            return new MessageSegment(new TestTextSegment(msg, anotherField));
        }
    }

    [TestMethod]
    public void TestGenericGet()
    {
        IMessageSegment seg = TestTextSegment.Build("test", "anotherField");
        Assert.AreEqual("text", seg.Type);
        Assert.AreEqual("anotherField", seg.Get<string>("another_field"));
    }

    [TestMethod]
    public void TestDictionaryGet()
    {
        var data = new SimpleDictionarySegment
        {
            ["field_a"] = "1",
            ["field_b"] = 2
        };
        var t = new MessageSegment("custom_type", data);
        Assert.AreEqual("custom_type", t.Type);
        Assert.AreEqual("1", t.Get<string>("field_a"));
        Assert.AreEqual(2, t.Get<int>("field_b"));
    }
}
