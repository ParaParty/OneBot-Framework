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
    public ValueTask Start()
    {
        _initializationManager.Initialize();
        _platformProviderManager.Start();
        return ValueTask.CompletedTask;
    }

    public ValueTask Stop()
    {
        _platformProviderManager.Stop();
        return ValueTask.CompletedTask;
    }
}
