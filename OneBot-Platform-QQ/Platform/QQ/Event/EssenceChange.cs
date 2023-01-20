using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.essence", "group")]
public interface EssenceChange : OneBotEvent, OneBotEvent.SubType
{
    new sealed class SubType
    {
        public const string Add = "add";

        public const string Delete = "delete";
    }

    string MessageId { get; }

    string GroupId { get; }

    string UserId { get; }

    string OperatorId { get; }
}

[OneBotTypeProperty("qq.file", "group")]
public interface FileUpload : OneBotEvent
{
    string GroupId { get; }

    string UserId { get; }

    string FileId { get; }

    string FileName { get; }

    long FileSize { get; }

    string Busid { get; }
}

[OneBotTypeProperty("qq.notice", "friend_request")]
public interface FriendRequest : OneBotEvent
{
    string UserId { get; }

    string Comment { get; }

    string RequestFlag { get; }
}

[OneBotTypeProperty("qq.notice", "group_admin_change")]
public interface GroupAdminChange : OneBotEvent, OneBotEvent.SubType
{
    new sealed class SubType
    {
        public const string Set = "set";

        public const string Unset = "unset";
    }
    
    string GroupId { get; }

    string UserId { get; }
}
