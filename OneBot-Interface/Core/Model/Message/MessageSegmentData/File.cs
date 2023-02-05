using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("file")]
public class File : Dictionary<string, object?>
{
    public File(string fileId)
    {
        Add("file_id", fileId);
    }
}
