using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using OneBot.Core.Context;

namespace OneBot.Core.Services;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)]
public interface IExceptionHandlerManager
{
    public ValueTask Handle(OneBotContext ctx, Exception exception);
}