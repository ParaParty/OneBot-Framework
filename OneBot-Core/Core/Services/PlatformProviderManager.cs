using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OneBot.Core.Configuration;
using OneBot.Core.Interface;

namespace OneBot.Core.Services;

public class PlatformProviderManager : IPlatformProviderManager
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<PlatformProviderManager> _logger;

    private readonly ImmutableDictionary<string, Type>  _providerTypeList;

    private readonly List<IPlatformProvider> _providerList;

    public PlatformProviderManager(OneBotConfiguration oneBotConfiguration, ILogger<PlatformProviderManager> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _providerTypeList = oneBotConfiguration.PlatformProviders;
        _providerList = new List<IPlatformProvider>();

        if (_providerTypeList.Count == 0)
        {
            _logger.LogError("no platform provider registered");
        }
    }
    public async ValueTask Start()
    {
        foreach (var item in _providerTypeList)
        {
            var instance = (IPlatformProvider)_serviceProvider.GetRequiredService(item.Value);
            await instance.Start();
            _providerList.Add(instance);
        }
    }

    public async ValueTask Stop()
    {
        foreach (var item in _providerList)
        {
            await item.Stop();
        }
    }
}
