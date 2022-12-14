namespace OneBot.CommandRoute.Model.Channel.Notice;

public interface ChannelMemberIncrease
{
    string DetailType { get; }
    string SubType { get; }
    string GuildId { get; }
    string UserId { get; }
    string OperatorId { get; }
}
