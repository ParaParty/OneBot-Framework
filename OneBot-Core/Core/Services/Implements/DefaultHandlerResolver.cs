using System;
using OneBot.Core.Context;
using OneBot.Core.Event;
using OneBot.Core.Interface;

namespace OneBot.Core.Services.Implements;

public class DefaultHandlerResolver : IHandlerResolver
{
    public bool Supports(OneBotContext ctx, Type handlerType)
    {
        if (!handlerType.IsConstructedGenericType)
        {
            return false;
        }

        var typeArgs = handlerType.GetGenericArguments();
        return typeArgs.Length == 1 && typeArgs[0].IsAssignableTo(typeof(OneBotEvent));
    }
}
