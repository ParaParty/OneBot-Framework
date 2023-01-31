using System.Threading.Tasks;
using OneBot.Core.Interface;
using OneBot.Core.Model;

namespace OneBot.Core.Util;

public static class PlatformActionEmitterExtension
{
    public static ValueTask<OneBotActionResponse> Emit(this IPlatformActionEmitter self, string platformName, string action, IOneBotActionRequestParams actionRequest)
    {
        return self.Emit(platformName, new OneBotActionRequest(action, actionRequest));
    }
}
