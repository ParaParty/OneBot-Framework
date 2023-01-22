using System;
using System.Threading.Tasks;
using OneBot.Core.Context;

namespace OneBot.Core.Services;

public interface IExceptionHandlerManager{
    public ValueTask Handle(OneBotContext ctx, Exception exception);
}