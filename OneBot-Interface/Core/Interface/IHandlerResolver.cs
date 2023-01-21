using System;
using System.Reflection;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

public interface IHandlerResolver
{
    bool Supports(OneBotContext ctx, Type handlerType, MethodInfo handler);
}
