﻿namespace OneBot.Core.Model.Channel.Action;

public interface GetGuildInfoResponse
{
    string GuildId { get; }

    string GuildName { get; }
}