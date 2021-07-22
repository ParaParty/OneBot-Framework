using System;
using Microsoft.Extensions.DependencyInjection;
using Sora.EventArgs.SoraEvent;

namespace OneBot_CommandRoute.CommandRoute.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class BeforeCommandAttribute : Attribute
    {
        public abstract bool Invoke(IServiceScope scope, BaseSoraEventArgs baseSoraEventArgs);
    }
}