using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

public interface FileUpload : OneBotEvent
{
    string FileName { get; }

    long FileSize { get; }
}