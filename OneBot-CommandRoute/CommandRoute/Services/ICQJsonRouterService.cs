using OneBot.CommandRoute.Attributes;
using OneBot.CommandRoute.Models;

using Sora.EventArgs.SoraEvent;

using System.Reflection;

namespace OneBot.CommandRoute.Services
{
    public interface ICQJsonRouterService
    {
        void Register(IOneBotController oneBotController, MethodInfo method, CQJsonAttribute attr);

        int Handle(OneBotContext scope, BaseSoraEventArgs eventArgs, string appid);
    }
}