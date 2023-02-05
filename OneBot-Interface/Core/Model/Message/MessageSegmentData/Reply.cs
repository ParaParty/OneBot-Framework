using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("reply")]
public class Reply : Dictionary<string, object?>
{
    public Reply(string messageId, string userId)
    {
        Add("message_id", messageId);
        Add("user_id", userId);
    }
}
