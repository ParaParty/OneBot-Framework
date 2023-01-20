using System;
using OneBot.Core.Context;

namespace OneBot.CommandRoute.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public abstract class BeforeCommandAttribute : Attribute
{
    public abstract void Invoke(OneBotContext scope);
}