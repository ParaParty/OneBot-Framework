using System;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.Core.Services.Implements;

public class OneBotContextHolder : IOneBotContextHolder
{
    public OneBotContext? OneBotContext { get; private set; }

    public void SetOneBotContext(OneBotContext context)
    {
        if (OneBotContext != null)
        {
            throw new ArgumentException();
        } 
        OneBotContext = context;
    }

    public OneBotContext RequireOneBotContext()
    {
        if (OneBotContext == null)
        {
            throw new NullReferenceException(
                "找不到 OneBotContext 对象。" +
                "如果您确认您确实是在 OneBot 上下文中使用本 Holder 的话，" +
                "由于 OneBotContextHolder 生命周期开始时机早于 OneBotContext，" +
                "本错误一般是在早于 OneBot 上下文产生时就调用本函数（比如说在构造函数注入时就使用）造成的。" +
                "可以考虑换为收到了 OneBot 事件之后（比如已经在处理事件了的时候）再取。"
                );
        }
        return OneBotContext;
    }
}
