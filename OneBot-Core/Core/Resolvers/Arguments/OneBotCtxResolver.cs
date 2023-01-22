using System;
using System.Reflection;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.Core.Resolvers.Arguments;

public sealed class OneBotCtxResolver : IArgumentResolver
{
    public bool SupportsParameter(Type handlerType, MethodInfo methodInfo, ParameterInfo parameterInfo)
    {
        return parameterInfo.GetType().IsAssignableFrom(typeof(OneBotContext));
    }

    public object ResolveArgument(OneBotContext ctx, Type handlerType, MethodInfo methodInfo, ParameterInfo parameterInfo)
    {
        return ctx;
    }
}
