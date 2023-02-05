using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("music")]
public class Music : Dictionary<string, object?>
{
    public Music(string musicType, string musicId)
    {
        Add("music_type", musicType);
        Add("music_id", musicId);
    }
}
