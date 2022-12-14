namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetChannelMemberListResponse : ChannelActionBasicRequest
{
    string ChannelId { get; }
}
