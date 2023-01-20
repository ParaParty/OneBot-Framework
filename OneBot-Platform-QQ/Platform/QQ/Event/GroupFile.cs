using OneBot.Core.Attributes;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.file", "group")]
public interface GroupFile: FileUpload
{
    string GroupId { get; }

    string UserId { get; }
    
    string FileId { get; }
    
    string Busid { get; }
}