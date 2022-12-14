namespace OneBot.CommandRoute.Model.Action;

public interface SendMessageRequest
{
    string DetailType { get; }
    string UserId { get; }
    string? GroupId { get; }
    string? GuildId { get; }
    string? ChannelId { get; }
    string Message { get; }
}
