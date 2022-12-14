namespace OneBot.Core.Model.Message.MessageSegment;

public interface MsgSegLocation
{
    float Latitude { get; }

    float Longitude { get; }

    string Title { get; }

    string Content { get; }
}
