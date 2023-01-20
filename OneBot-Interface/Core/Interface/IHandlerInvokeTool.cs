using System;
using System.Threading.Tasks;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

public interface IHandlerInvokeTool
{
    ValueTask Invoke(OneBotContext ctx, Type handlerType);
}
