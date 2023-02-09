namespace OneBot.Core.Model.Message.Action;

public class SendMessageRequest : IOneBotActionRequestParams
{
    public SendMessageRequest(string detailType, Message message, string? userId = null, string? groupId = null, string? guildId = null, string? channelId = null)
    {
        DetailType = detailType;
        UserId = userId;
        GroupId = groupId;
        GuildId = guildId;
        ChannelId = channelId;
        Message = message;
    }

    string DetailType { get; }

    string? UserId { get; }

    string? GroupId { get; }

    string? GuildId { get; }

    string? ChannelId { get; }

    Message Message { get; }
}
