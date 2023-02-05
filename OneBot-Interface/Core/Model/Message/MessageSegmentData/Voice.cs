using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("voice")]
public class Voice : Dictionary<string, object?>
{
    public Voice(string fileId)
    {
        Add("file_id", fileId);
    }
}
