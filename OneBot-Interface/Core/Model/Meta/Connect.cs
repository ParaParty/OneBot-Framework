namespace OneBot.Core.Model.Meta;

public interface Connect : OneBotEvent
{
    ConnVersion Version { get; }

    public interface ConnVersion
    {
        string Impl { get; }

        string Version { get; }

        string OnebotVersion { get; }
    }
}
