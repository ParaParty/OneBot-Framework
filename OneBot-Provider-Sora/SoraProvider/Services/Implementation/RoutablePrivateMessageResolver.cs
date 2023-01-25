using System;
using OneBot.Core.Context;
using OneBot.Core.Interface;
using OneBot.Core.Model.CommandRoute;
using OneBot.Provider.SoraProvider.Model;

namespace OneBot.Provider.SoraProvider.Services.Implementation;

public class RoutablePrivateMessageResolver: IRoutableMessageResolver
{

    public bool SupportEvent(Type type)
    {
        if (type.IsAssignableTo(typeof(SoraSelfPrivateMessage)))
        {
            return false;
        }
        return type.IsAssignableTo(typeof(SoraPrivateMessage));
    }

    public RouteInfo ResolveMessage(OneBotContext ctx)
    {
        if (ctx.Event is SoraPrivateMessage e)
        {
            return new RouteInfo
            {
                Message = e.Message,
                EventType = EventType.PrivateMessage,
            };
        }
        throw new InvalidCastException();
    }
}
