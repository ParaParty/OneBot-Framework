﻿using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Channel.Notice;

[OneBotTypeProperty("notice", "channel_message_delete")]
public interface ChannelMessageDelete: OneBotEvent, OneBotEvent.SubType
{
    string GuildId { get; }

    string ChannelId { get; }

    string MessageId { get; }

    string UserId { get; }

    string OperatorId { get; }
}
