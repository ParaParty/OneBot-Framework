namespace OneBot.CommandRoute.Model.Channel.Action;

public interface SetGuildNameRequest : ChannelActionBasicRequest
{
    string GuildName { get; }
}
