namespace OneBot.Core.Model.Meta;

public interface Connect : OneBotEvent
{
    Version version { get; }

    public interface Version
    {
        string Impl { get; }

        string Version { get; }

        string OnebotVersion { get; }
    }
}
