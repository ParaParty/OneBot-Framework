namespace OneBot.Core.Event;

public interface OneBotEvent
{
    string Id { get; }

    double Time { get; }

    public interface Type
    {
        const string PropertyName = "type";
        
        string Type { get; }
    }

    public interface DetailType
    {
        const string PropertyName = "detail_type";
        
        string DetailType { get; }
    }

    public interface SubType
    {
        const string PropertyName = "sub_type";
        
        string SubType { get; }
    }
}
