using System;
using OneBot.Core.Context;
using OneBot.Core.Model.CommandRoute;

namespace OneBot.Core.Interface;

public interface IRoutableMessageResolver
{
    bool SupportEvent(Type type);
    
    RouteInfo ResolveMessage(OneBotContext ctx);
}
