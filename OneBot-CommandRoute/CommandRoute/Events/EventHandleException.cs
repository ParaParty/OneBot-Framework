using System;

namespace OneBot.Core.Events;

internal class EventHandleException : Exception
{
    public EventHandleException(string msg) : base(msg)
    {
    }
}