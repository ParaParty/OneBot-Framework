using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("red_bag")]
public class RedBag : Dictionary<string, object?>
{
    public RedBag(string title)
    {
        Add("title", title)
    }
}
