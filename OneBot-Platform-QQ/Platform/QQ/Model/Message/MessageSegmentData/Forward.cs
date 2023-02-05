using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("forward")]
public class Forward : Dictionary<string, object?>
{
    public Forward(string messageId)
    {
        Add("message_id", messageId);
    }
}
