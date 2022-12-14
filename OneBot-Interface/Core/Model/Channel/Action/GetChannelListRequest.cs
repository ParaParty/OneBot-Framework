namespace OneBot.Core.Model.Channel.Action;

public interface GetChannelListRequest : ChannelActionBasicRequest
{
    bool JoinedOnly { get; }
}
