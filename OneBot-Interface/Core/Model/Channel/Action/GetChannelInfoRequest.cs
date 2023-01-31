namespace OneBot.Core.Model.Channel.Action;

public class GetChannelInfoRequest : IOneBotActionRequestParams
{
    public GetChannelInfoRequest(string guildId, string channelId)
    {
        GuildId = guildId;
        ChannelId = channelId;
    }

    string GuildId { get; }

    string ChannelId { get; }
}
