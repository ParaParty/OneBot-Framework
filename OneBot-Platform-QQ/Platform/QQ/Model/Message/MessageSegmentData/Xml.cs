using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("xml")]
public class Xml : Dictionary<string, object?>
{
    public Xml(string content)
    {
        Add("content", content);
    }
}
