using System.Collections.Generic;
using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("location")]
public class Location : Dictionary<string, object?>
{
    public Location(double latitude, double longitude, string title, string content)
    {
        Add("latitude", latitude);
        Add("longitude", longitude);
        Add("title", title);
        Add("content", content);
    }
}
