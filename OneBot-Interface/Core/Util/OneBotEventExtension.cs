using System;
using OneBot.Core.Event;
using OneBot.Core.Model;

namespace OneBot.Core.Util;

public static class OneBotEventExtension
{
    public static T Unwrap<T>(this OneBotEvent self)
    {
        if (self is not UnderlayModel<T> p)
        {
            throw new InvalidCastException();
        }
        return p.WrappedModel;
    }
}
