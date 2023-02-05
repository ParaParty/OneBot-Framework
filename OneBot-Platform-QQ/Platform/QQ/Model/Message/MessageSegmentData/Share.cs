using System.Collections.Generic;
using System.Runtime.Serialization;
using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Model.Message.MessageSegmentData;

[OneBotTypeProperty("share")]
public class Share : Dictionary<string, object?>
{
    public Share(string url, string title, string content, string imageUrl)
    {
        Add("url", url);
        Add("title", title);
        Add("content", content);
        Add("image_url", imageUrl);
    }

}
