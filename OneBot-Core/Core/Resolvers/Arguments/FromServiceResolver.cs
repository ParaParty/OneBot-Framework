using System;
using System.Linq;
using System.Reflection;
using OneBot.Core.Attributes;
using OneBot.Core.Context;
using OneBot.Core.Interface;

namespace OneBot.Core.Resolvers.Arguments;

public sealed class FromServiceResolver : IArgumentResolver
{
    public bool SupportsParameter(Type handlerType, MethodInfo methodInfo, ParameterInfo parameterInfo)
    {
        return parameterInfo.GetCustomAttributes().Any(s =>
        {
            var t = s.GetType();
            if (t.IsAssignableTo(typeof(FromServicesAttribute)))
            {
                return true;
            }
            return t is { Namespace: "Microsoft.AspNetCore.Mvc", Name: "FromServicesAttribute" };
        });
    }

    public object? ResolveArgument(OneBotContext ctx, Type handlerType, MethodInfo methodInfo, ParameterInfo parameterInfo)
    {
        return ctx.ServiceScope.ServiceProvider.GetService(parameterInfo.ParameterType);
    }
}
