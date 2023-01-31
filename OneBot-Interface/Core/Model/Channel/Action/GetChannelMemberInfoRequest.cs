namespace OneBot.Core.Model.Channel.Action;

public class GetChannelMemberInfoRequest : IOneBotActionRequestParams
{
    public GetChannelMemberInfoRequest(string guildId, string channelId, string userId)
    {
        GuildId = guildId;
        ChannelId = channelId;
        UserId = userId;
    }

    string GuildId { get; }

    string ChannelId { get; }

    string UserId { get; }
}
