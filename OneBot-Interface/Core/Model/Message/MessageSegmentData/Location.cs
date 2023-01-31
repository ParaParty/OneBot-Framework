using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Message.MessageSegmentData;

[OneBotTypeProperty("location")]
public interface Location : MessageData
{
    double Latitude { get; }

    double Longitude { get; }

    string Title { get; }

    string Content { get; }
}
