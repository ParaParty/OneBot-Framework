using OneBot.Core.Attributes;
using OneBot.Core.Event;

namespace OneBot.Platform.QQ.Event;

[OneBotTypeProperty("qq.notice", "join_group_request")]
public interface AddGroupRequest : OneBotEvent, OneBotEvent.SubType
{
    new sealed class SubType
    {
        public const string Add = "add";

        public const string Invite = "invite";
    }

    string UserId { get; }

    string InvitorUserId { get; }

    string GroupId { get; }

    string Comment { get; }

    string RequestFlag { get; }
}