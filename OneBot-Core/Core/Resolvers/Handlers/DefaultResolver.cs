﻿using System;
using System.Reflection;
using OneBot.Core.Context;
using OneBot.Core.Event;
using OneBot.Core.Interface;

namespace OneBot.Core.Resolvers.Handlers;

public class DefaultResolver : IHandlerResolver
{
    public bool Supports(OneBotContext ctx, Type handlerType, MethodInfo handler)
    {
        var typeArgs = handler.GetParameters();
        return typeArgs.Length == 1 && typeArgs[0].ParameterType.IsAssignableTo(typeof(OneBotEvent));
    }
}