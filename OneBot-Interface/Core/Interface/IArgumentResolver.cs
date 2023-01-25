using System;
using System.Reflection;
using JetBrains.Annotations;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IArgumentResolver
{
    bool SupportsParameter(Type? handlerType, MethodInfo methodInfo, ParameterInfo parameterInfo);

    object? ResolveArgument(OneBotContext ctx, Type? handlerType, MethodInfo methodInfo, ParameterInfo parameterInfo);
}
