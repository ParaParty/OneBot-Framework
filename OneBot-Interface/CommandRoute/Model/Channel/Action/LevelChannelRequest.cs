namespace OneBot.CommandRoute.Model.Channel.Action;

public interface LevelChannelRequest : ChannelActionBasicRequest
{
    string ChannelId { get; }
}
