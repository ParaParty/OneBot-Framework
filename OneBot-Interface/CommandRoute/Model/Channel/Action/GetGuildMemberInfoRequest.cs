namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetGuildMemberInfoRequest : ChannelActionBasicRequest
{
    string UserId { get; }
}
