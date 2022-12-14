namespace OneBot.Core.Model.Channel.Action;

public interface SetGuildNameRequest : ChannelActionBasicRequest
{
    string GuildName { get; }
}
