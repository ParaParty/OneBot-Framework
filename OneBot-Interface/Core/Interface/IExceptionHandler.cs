using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

[UsedImplicitly(Const.Flags.AllImplicitUseTargetFlags)] 
public interface IExceptionHandler
{
    ValueTask Handle(OneBotContext ctx, Exception exception);
}
