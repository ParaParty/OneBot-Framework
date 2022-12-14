namespace OneBot.Core.Model.Channel.Notice;

public interface ChannelCreate
{
    string DetailType { get; }

    string GuildId { get; }

    string ChannelId { get; }

    string OperatorId { get; }
}
