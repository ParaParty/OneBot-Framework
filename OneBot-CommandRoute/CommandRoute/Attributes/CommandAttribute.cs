using System;
using OneBot.CommandRoute.Models.Enumeration;

namespace OneBot.CommandRoute.Attributes
{
    /// <summary>
    /// 指令路由
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// 指令格式
        /// </summary>
        public string Pattern { get; private set; } = "";

        /// <summary>
        /// 指令别名
        /// </summary>
        public string[] Alias { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType EventType { get; set; } = EventType.GroupMessage;

        /// <summary>
        /// 指令路由
        /// </summary>
        /// <param name="pattern">指令格式</param>
        public CommandAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}