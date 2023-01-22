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

    private readonly ImmutableArray<Type> _providerTypeList;

    private readonly List<IPlatformProvider> _providerList;

    public PlatformProviderManager(OneBotConfiguration oneBotConfiguration, ILogger<PlatformProviderManager> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _providerTypeList = oneBotConfiguration.PlatformProviders;
        _providerList = new List<IPlatformProvider>();

        if (_providerTypeList.Length == 0)
        {
            _logger.LogError("no platform provider registered");
        }
    }
    public ValueTask Start()
    {
        foreach (var item in _providerTypeList)
        {
            var instance = ((IPlatformProvider)_serviceProvider.GetRequiredService(item))!;
            instance.Start();
            _providerList.Add(instance);
        }
        return ValueTask.CompletedTask;
    }

    public ValueTask Stop()
    {
        foreach (var item in _providerList)
        {
            item.Stop();
        }
        return ValueTask.CompletedTask;
    }
}
