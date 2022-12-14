namespace OneBot.CommandRoute.Model.Channel.Action;

public interface SetChannelNameRequest : ChannelActionBasicRequest
{
    string ChannelId { get; }
    string ChannelName { get; }
}
