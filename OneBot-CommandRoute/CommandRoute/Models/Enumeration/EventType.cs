using System;

namespace OneBot.CommandRoute.Models.Enumeration
{
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
        DM = 0x4
    }
}