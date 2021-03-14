using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OneBot.CommandRoute.Attributes;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Services
{
    public interface ICQJsonRouterService
    {
        void Register(IOneBotController oneBotController, MethodInfo method, CQJsonAttribute attr);
        
        int Handle(IServiceScope scope, BaseSoraEventArgs eventArgs, string appid);
    }
}