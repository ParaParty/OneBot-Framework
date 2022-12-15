namespace OneBot.Core.Model;

public interface OneBotEvent
{
    string Id { get; }

    double Time { get; }

    string Type { get; }

    string DetailType { get; }

    string SubType { get; } 
}
