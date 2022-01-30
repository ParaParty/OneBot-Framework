using System;

namespace OneBot.CommandRoute.Events;

internal class EventHandleException : Exception
{
    public EventHandleException(string msg) : base(msg)
    {
    }
}