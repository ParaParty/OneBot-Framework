using System;

namespace OneBot.CommandRoute.Events
{
    class EventHandleException : Exception
    {
        public EventHandleException(string msg): base(msg)
        {
            
        }
    }
}