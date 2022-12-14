namespace OneBot.Core.Model.Channel.Action;

public interface GetGuildMemberInfoRequest : ChannelActionBasicRequest
{
    string UserId { get; }
}
