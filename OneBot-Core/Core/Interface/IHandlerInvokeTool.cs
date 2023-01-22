using System;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IHandlerInvokeTool
{
    ValueTask Invoke(OneBotContext ctx, Type handlerType, MethodInfo handlerMethod);
}
