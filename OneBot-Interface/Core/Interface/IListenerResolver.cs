using System;
using OneBot.Core.Context;

namespace OneBot.Core.Interface;

public interface IListenerResolver
{
    bool Supports(OneBotContext ctx, Type handlerType);
}
