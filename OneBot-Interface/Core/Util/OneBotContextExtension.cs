using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Context;
using OneBot.Core.Interface;
using OneBot.Core.Model;
using OneBot.Core.Model.Message;

namespace OneBot.Core.Util;

public static class OneBotContextExtension
{
    public static ValueTask<OneBotActionResponse> EmitEvent(this OneBotContext self, OneBotActionRequest args)
    {
        var emitter = self.ServiceScope.ServiceProvider.GetRequiredService<IPlatformActionEmitter>();
        return emitter.Emit(self.PlatformProviderName, args);
    }

    public static ValueTask<OneBotActionResponse> EmitEvent(this OneBotContext self, string action, IOneBotActionRequestParams args)
    {
        var emitter = self.ServiceScope.ServiceProvider.GetRequiredService<IPlatformActionEmitter>();
        return emitter.Emit(self.PlatformProviderName, action, args);
    }
    
    public static ValueTask<OneBotActionResponse> Reply(this OneBotContext self, Message msg)
    {
        throw new NotImplementedException();
    }
}
