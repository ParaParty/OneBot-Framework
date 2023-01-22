using System;
using System.Reflection;
using JetBrains.Annotations;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IHandlerResolver
{
    bool Supports(OneBotContext ctx, Type handlerType, MethodInfo handler);
}
