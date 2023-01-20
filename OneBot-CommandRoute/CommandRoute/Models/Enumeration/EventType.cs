using System;

namespace OneBot.CommandRoute.Models.Enumeration;

/// <summary>
/// 指令类型
/// </summary>
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
    // ReSharper disable once InconsistentNaming
    PM = PrivateMessage,

    /// <summary>
    /// 群聊
    /// </summary>
    GroupMessage = 0x2,

    /// <summary>
    /// 群聊
    /// </summary>
    // ReSharper disable once InconsistentNaming
    GM = GroupMessage,

    /// <summary>
    /// 讨论组
    /// </summary>
    [Obsolete] DiscussMessage = 0x4,

    /// <summary>
    /// 讨论组
    /// </summary>
    // ReSharper disable once InconsistentNaming
    [Obsolete] DM = DiscussMessage
}