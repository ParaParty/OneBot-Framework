using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("face")]
public class Face : Dictionary<string, object?>
{
    public Face(string faceId)
    {
        Add("face_id", faceId);
    }
}
