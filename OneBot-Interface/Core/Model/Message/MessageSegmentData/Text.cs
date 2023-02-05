using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("text")]
public class Text : Dictionary<string, object?>
{
    public Text(string text)
    {
        Add("text", text);
    }
}
