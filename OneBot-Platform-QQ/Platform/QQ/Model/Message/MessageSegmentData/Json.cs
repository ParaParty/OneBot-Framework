using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("json")]
public class Json : Dictionary<string, object?>
{
    public Json(string content)
    {
        Add("content", content);
    }
}
