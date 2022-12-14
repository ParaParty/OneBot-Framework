namespace OneBot.CommandRoute.Model.Channel.Notice;

public interface ChannelMessageDelete
{
    string DetailType { get; }
    string SubType { get; }
    string GuildId { get; }
    string ChannelId { get; }
    string MessageId { get; }
    string UserId { get; }
    string OperatorId { get; }
}
