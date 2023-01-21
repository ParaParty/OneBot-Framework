using System;
using System.Reflection;
using OneBot.Core.Context;
using OneBot.Core.Event;
using OneBot.Core.Interface;

namespace OneBot.Core.Services.Implements;

public class DefaultHandlerResolver : IHandlerResolver
{
    public bool Supports(OneBotContext ctx, Type handlerType, MethodInfo handler)
    {
        var typeArgs = handler.GetParameters();
        return typeArgs.Length == 1 && typeArgs[0].ParameterType.IsAssignableTo(typeof(OneBotEvent));
    }
}
