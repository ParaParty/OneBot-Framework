using System;
using System.Threading.Tasks;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

public interface IExceptionHandler
{
    ValueTask Handle(OneBotContext ctx, Exception exception);
}
