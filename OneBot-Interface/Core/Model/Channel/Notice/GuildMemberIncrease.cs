namespace OneBot.Core.Model.Channel.Notice;

public interface GuildMemberIncrease
{
    string DetailType { get; }

    string SubType { get; }

    string GuildId { get; }

    string ChannelId { get; }

    string UserId { get; }

    string OperatorId { get; }

}
