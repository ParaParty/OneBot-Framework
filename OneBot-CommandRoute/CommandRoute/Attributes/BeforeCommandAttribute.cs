using System;
using OneBot.CommandRoute.Models;

namespace OneBot.CommandRoute.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public abstract class BeforeCommandAttribute : Attribute
{
    public abstract void Invoke(OneBotContext scope);
}