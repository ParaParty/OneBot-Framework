using OneBot.CommandRoute.Models;

namespace OneBot.CommandRoute.Services;

/// <summary>
/// OneBot 上下文持有器
/// </summary>
public interface IOneBotContextHolder
{
    /// <summary>
    /// 获取 OneBot 上下文，若无则返回 Null。
    /// </summary>
    public OneBotContext? OneBotContext { get; }

    /// <summary>
    /// 获取 OneBot 上下文，若无则抛出错误。
    /// </summary>
    public OneBotContext RequireOneBotContext();
}
