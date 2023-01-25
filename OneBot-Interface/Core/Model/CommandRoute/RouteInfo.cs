namespace OneBot.Core.Model.CommandRoute;

public struct RouteInfo
{
    public Message.Message Message { get; set; }

    public EventType EventType { get; set; }
}
