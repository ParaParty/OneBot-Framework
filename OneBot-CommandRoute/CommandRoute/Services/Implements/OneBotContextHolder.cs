using System;
using OneBot.CommandRoute.Models;

namespace OneBot.CommandRoute.Services.Implements;

public class OneBotContextHolder : IOneBotContextHolder
{
    public OneBotContext? OneBotContext { get; private set; }

    internal void SetOneBotContext(OneBotContext context)
    {
        OneBotContext = context;
    }

    public OneBotContext RequireOneBotContext()
    {
        if (OneBotContext == null)
        {
            throw new NullReferenceException(
                @"找不到 OneBotContext 对象。
如果您确认您确实是在 OneBot 上下文中使用本 Holder 的话，
这个错误这是因为 OneBotContextHolder 实例化时间早于 OneBotContext 造成的，
因此若在构造函数注入时就取 OneBotContext 会导致取不出。
可以考虑换为收到了 OneBot 事件之后（比如已经在处理事件了的时候）再取。");
        }
        return OneBotContext;
    }
}
