using System;

namespace OneBot.Core.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public abstract class BeforeCommandAttribute : Attribute
{
    public abstract void Invoke(OneBotContext scope);
}