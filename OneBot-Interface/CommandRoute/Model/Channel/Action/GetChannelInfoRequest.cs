namespace OneBot.CommandRoute.Model.Channel.Action;

public interface GetChannelInfoRequest : ChannelActionBasicRequest
{
    string ChannelId { get; }
}
