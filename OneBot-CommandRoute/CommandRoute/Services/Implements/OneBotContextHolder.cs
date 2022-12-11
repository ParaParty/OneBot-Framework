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
出现这个错误是因为 OneBotContextHolder 实例化时间早于 OneBotContext，
导致在早于 OneBot 上下文产生之前就使用这个函数（比如说在
构造函数注入时就使用）就会出现这个错误。可以考虑换为收到
了 OneBot 事件之后（比如已经在处理事件了的时候）再取。"
                );
        }
        return OneBotContext;
    }
}
