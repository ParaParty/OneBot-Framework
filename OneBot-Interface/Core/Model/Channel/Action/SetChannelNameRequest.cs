namespace OneBot.Core.Model.Channel.Action;

public class SetChannelNameRequest : IOneBotActionRequestParams
{
    public SetChannelNameRequest(string guildId, string channelId, string channelName)
    {
        GuildId = guildId;
        ChannelId = channelId;
        ChannelName = channelName;
    }

    string GuildId { get; }

    string ChannelId { get; }

    string ChannelName { get; }
}
