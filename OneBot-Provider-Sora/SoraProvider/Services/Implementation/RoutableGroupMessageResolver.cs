using System;
using OneBot.Core.Context;
using OneBot.Core.Interface;
using OneBot.Core.Model.CommandRoute;
using OneBot.Provider.SoraProvider.Model;

namespace OneBot.Provider.SoraProvider.Services.Implementation;

public class RoutableGroupMessageResolver : IRoutableMessageResolver
{
    public bool SupportEvent(Type type)
    {
        if (type.IsAssignableTo(typeof(SoraSelfGroupMessage)))
        {
            return false;
        }
        return type.IsAssignableTo(typeof(SoraGroupMessage));
    }

    public RouteInfo ResolveMessage(OneBotContext ctx)
    {
        if (ctx.Event is SoraGroupMessage e)
        {
            return new RouteInfo(
                message: e.Message,
                eventType: EventType.GroupMessage
            );
        }

        throw new InvalidCastException();
    }
}
