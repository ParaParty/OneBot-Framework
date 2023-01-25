using System.Threading.Tasks;
using OneBot.Core.Interface;

namespace OneBot.Core.Services;

public class OneBotService : IOneBotService
{
    private readonly IPlatformProviderManager _platformProviderManager;

    private readonly IInitializationManager _initializationManager;

    public OneBotService(IPlatformProviderManager platformProviderManager, IInitializationManager initializationManager)
    {
        _platformProviderManager = platformProviderManager;
        _initializationManager = initializationManager;
    }
    public async ValueTask Start()
    {
        _initializationManager.Initialize();
        await _platformProviderManager.Start();
    }

    public async ValueTask Stop()
    {
        await _platformProviderManager.Stop();
    }
}
