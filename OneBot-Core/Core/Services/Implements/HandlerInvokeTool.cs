using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.Core.Services.Implements;

public class HandlerInvokeTool: IHandlerInvokeTool
{
    private ConcurrentDictionary<Type, MethodInfo> handlerMethod = new ConcurrentDictionary<Type, MethodInfo>();

    public ValueTask Invoke(OneBotContext ctx, Type handlerType)
    {
        handlerMethod.
    }
}
