﻿using System;

namespace OneBot.Core.Model.CommandRoute;

[Flags]
public enum EventType
{
    /// <summary>
    /// 私聊
    /// </summary>
    PrivateMessage = 0x1,

    /// <summary>
    /// 私聊
    /// </summary>
    PM = 0x1,

    /// <summary>
    /// 群聊
    /// </summary>
    GroupMessage = 0x2,

    /// <summary>
    /// 群聊
    /// </summary>
    GM = 0x2,

    /// <summary>
    /// 讨论组
    /// </summary>
    DiscussMessage = 0x4,

    /// <summary>
    /// 讨论组
    /// </summary>
    DM = 0x4,

    /// <summary>
    /// 频道
    /// </summary>
    ChannelMessage = 0x8,
    
    /// <summary>
    /// 频道
    /// </summary>
    GuildMessage = 0x8,
}
