namespace OneBot.CommandRoute.Model.Channel;

public interface ChannelMessage
{
    string DetailType { get; init; }
    string MessageId { get; }
    string Message { get; }
    string AltMessage { get; }
    string GuildId { get; }
    string ChannelId { get; }
    string UserId { get; }
}
