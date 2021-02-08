using System;
using QQRobot.CommandRoute;

namespace QQRobot.Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : System.Attribute
    {
        public string Pattern { get; private set; } = "";
        public string Alias { get; set; } = "";
        public EventType EventType { get; set; } = EventType.GroupMessage;

        public CommandAttribute(String pattern)
        {
            Pattern = pattern;
        }
    }
}