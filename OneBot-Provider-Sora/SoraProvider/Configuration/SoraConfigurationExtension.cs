using System;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Configuration;
using OneBot.Core.Interface;
using OneBot.Provider.SoraProvider.Services.Implementation;

namespace OneBot.Provider.SoraProvider.Configuration;

public static class SoraConfigurationExtension
{
    public static void AddSora(this OneBotBuilder builder, string name, Func<IServiceProvider, SoraConfiguration, SoraConfiguration> closure)
    {
        var services = builder.Services;
        builder.AddPlatformProvider<SoraPlatformProviderService>(name);
        services.AddSingleton<SoraConfiguration>(s => closure(s, new SoraConfiguration(name)));

        services.AddSingleton<IRoutableMessageResolver, RoutablePrivateMessageResolver>();
        services.AddSingleton<IRoutableMessageResolver, RoutableGroupMessageResolver>();
    }
}
