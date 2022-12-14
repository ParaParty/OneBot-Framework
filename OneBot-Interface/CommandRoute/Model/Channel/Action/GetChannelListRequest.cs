namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetChannelListRequest : ChannelActionBasicRequest
{
    bool JoinedOnly { get; }
}
