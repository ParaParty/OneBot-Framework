using System;
using System.Reflection;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

public interface IArgumentResolver
{
    bool SupportsParameter(Type handlerType, MethodInfo methodInfo, ParameterInfo parameterInfo);

    object ResolveArgument(OneBotContext ctx, Type handlerType, MethodInfo methodInfo, ParameterInfo parameterInfo);
}
