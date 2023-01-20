namespace OneBot.Core.Event;

public interface OneBotEvent
{
    string Id { get; }

    double Time { get; }

    public interface Type
    {
        string Type { get; }
    }

    public interface DetailType
    {
        string DetailType { get; }
    }

    public interface SubType
    {
        string SubType { get; }
    }
}
