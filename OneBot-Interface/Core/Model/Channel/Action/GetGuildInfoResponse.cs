namespace OneBot.Core.Model.Channel.Action;

public class GetGuildInfoResponse : IOneBotActionResponseData
{
    public GetGuildInfoResponse(string guildId, string guildName)
    {
        GuildId = guildId;
        GuildName = guildName;
    }

    string GuildId { get; }

    string GuildName { get; }
}
