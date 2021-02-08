using System;

namespace QQRobot.CommandRoute.Events
{
    class EventHandleException : Exception
    {
        public EventHandleException(string msg): base(msg)
        {
            
        }
    }
}