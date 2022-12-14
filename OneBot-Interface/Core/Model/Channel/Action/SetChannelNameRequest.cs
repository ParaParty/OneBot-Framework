namespace OneBot.Core.Model.Channel.Action;

public interface SetChannelNameRequest : ChannelActionBasicRequest
{
    string ChannelId { get; }

    string ChannelName { get; }
}
