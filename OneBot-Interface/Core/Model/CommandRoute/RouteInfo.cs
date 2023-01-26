namespace OneBot.Core.Model.CommandRoute;

public class RouteInfo
{
    public RouteInfo(Message.Message message, EventType eventType, int startSegment = 0, int startPosition = 0)
    {
        Message = message;
        EventType = eventType;
        StartSegment = startSegment;
        StartPosition = startPosition;
    }

    public Message.Message Message { get; init; }

    public EventType EventType { get; init; }

    public int StartSegment { get; init; } = 0;

    public int StartPosition { get; init; } = 0;
}
