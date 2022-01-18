using System;
using Microsoft.Extensions.DependencyInjection;
using OneBot.CommandRoute.Models;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class BeforeCommandAttribute : Attribute
    {
        public abstract void Invoke(OneBotContext scope);
    }
}