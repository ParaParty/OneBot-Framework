namespace OneBot.Core.Model.Channel.Action;

public class GetChannelMemberListRequest : IOneBotActionRequestParams
{
    public GetChannelMemberListRequest(string guildId, string channelId)
    {
        GuildId = guildId;
        ChannelId = channelId;
    }

    string GuildId { get; }

    string ChannelId { get; }
}
