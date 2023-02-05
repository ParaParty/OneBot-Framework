using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("poke")]
public class Poke : Dictionary<string, object?>
{
    public Poke(string userId)
    {
        Add("user_id", userId);
    }
}
