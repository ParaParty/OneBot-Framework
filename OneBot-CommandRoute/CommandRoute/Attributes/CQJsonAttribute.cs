using System;
using OneBot.CommandRoute.Models.Enumeration;

namespace OneBot.CommandRoute.Attributes;

/// <summary>
/// CQ:Json 路由
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CQJsonAttribute : Attribute
{
    /// <summary>
    /// 指令格式
    /// </summary>
    public string AppId { get; }

    /// <summary>
    /// 事件类型
    /// </summary>
    public EventType EventType { get; set; } = EventType.GroupMessage;

    /// <summary>
    /// CQ:Json 路由
    /// </summary>
    /// <param name="appId">AppId</param>
    public CQJsonAttribute(string appId)
    {
        AppId = appId;
    }
}