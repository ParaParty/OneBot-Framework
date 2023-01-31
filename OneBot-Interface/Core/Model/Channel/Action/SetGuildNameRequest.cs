namespace OneBot.Core.Model.Channel.Action;

public class SetGuildNameRequest : IOneBotActionRequestParams
{
    public SetGuildNameRequest(string guildId, string guildName)
    {
        GuildId = guildId;
        GuildName = guildName;
    }

    string GuildId { get; }

    string GuildName { get; }
}
