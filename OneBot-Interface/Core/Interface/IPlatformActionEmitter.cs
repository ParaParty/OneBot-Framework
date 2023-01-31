using System.Threading.Tasks;
using OneBot.Core.Model;

namespace OneBot.Core.Interface;

public interface IPlatformActionEmitter
{
    public ValueTask<OneBotActionResponse> Emit(string platformName, OneBotActionRequest actionRequest);
}
