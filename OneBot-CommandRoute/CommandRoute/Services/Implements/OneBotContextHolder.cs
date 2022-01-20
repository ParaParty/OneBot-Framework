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
            throw new ArgumentNullException();
        }
        return OneBotContext;
    }
}
