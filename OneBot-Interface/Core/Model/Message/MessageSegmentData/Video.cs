using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("video")]
public class Video : Dictionary<string, object?>
{
    public Video(string fileId)
    {
        Add("file_id", fileId);
    }
}
