using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("tts")]
public class Tts : Dictionary<string, object?>
{
    public Tts(string content)
    {
        Add("content", content);
    }
}
