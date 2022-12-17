namespace OneBot.Core.Model.Message.MessageSegmentData;

public interface Location: MessageData
{
    float Latitude { get; }

    float Longitude { get; }

    string Title { get; }

    string Content { get; }
}
