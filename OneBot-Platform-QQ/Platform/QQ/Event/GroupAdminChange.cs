using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

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