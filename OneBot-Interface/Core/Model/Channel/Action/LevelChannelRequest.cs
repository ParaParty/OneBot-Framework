namespace OneBot.Core.Model.Channel.Action;

public interface LevelChannelRequest : ChannelActionBasicRequest
{
    string ChannelId { get; }
}
