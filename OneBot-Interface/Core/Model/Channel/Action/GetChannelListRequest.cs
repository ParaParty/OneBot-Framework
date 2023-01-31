namespace OneBot.Core.Model.Channel.Action;

public class GetChannelListRequest : IOneBotActionRequestParams
{
    public GetChannelListRequest(string guildId, bool joinedOnly)
    {
        GuildId = guildId;
        JoinedOnly = joinedOnly;
    }

    string GuildId { get; }

    bool JoinedOnly { get; }
}
