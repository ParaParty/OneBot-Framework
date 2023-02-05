using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("mention")]
public class Mention : Dictionary<string, object?>
{
    public Mention(string userId)
    {
        Add("user_id", userId);
    }
}
