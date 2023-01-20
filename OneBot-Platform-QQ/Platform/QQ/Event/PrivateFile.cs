using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.file", "private")]
public interface PrivateFile: FileUpload
{
    string UserId { get; }
    
    string Url { get; }
}