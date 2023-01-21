using System;

namespace OneBot.Core.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class OneBotEventHandlerAttribute : Attribute
{

}
