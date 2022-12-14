namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetChannelMemberInfoRequest : ChannelActionBasicRequest
{
    string ChannelId { get; }
    string UserId { get; }
}
