namespace OneBot.Core.Model.Channel.Action;

public interface GetChannelInfoRequest : ChannelActionBasicRequest
{
    string ChannelId { get; }
}
