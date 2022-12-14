namespace OneBot.Core.Model.Channel.Notice;

public interface GuildMemberDecrease
{
    string DetailType { get; }

    string SubType { get; }

    string GuildId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
