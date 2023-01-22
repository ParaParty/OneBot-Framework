using OneBot.Core.Context;

namespace OneBot.Core.Interface;

/// <summary>
/// OneBot 上下文持有器
/// </summary>
public interface IOneBotContextHolder
{
    /// <summary>
    /// 设置 OneBot 上下文。
    /// </summary>
    public void SetOneBotContext(OneBotContext context);
    
    /// <summary>
    /// 获取 OneBot 上下文，若无则返回 Null。
    /// </summary>
    public OneBotContext? OneBotContext { get; }

    /// <summary>
    /// 获取 OneBot 上下文，若无则抛出错误。
    /// </summary>
    public OneBotContext RequireOneBotContext();
}
