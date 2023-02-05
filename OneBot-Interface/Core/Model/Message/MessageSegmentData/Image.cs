using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("image")]
public class Image : Dictionary<string, object?>
{
    public Image(string fileId)
    {
        Add("file_id", fileId);
    }
}
