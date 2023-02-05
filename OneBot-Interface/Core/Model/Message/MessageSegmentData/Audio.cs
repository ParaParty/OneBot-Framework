using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("audio")]
public class Audio : Dictionary<string, object?>
{
    public Audio(string fileId)
    {
        Add("file_id", fileId);
    }
}
