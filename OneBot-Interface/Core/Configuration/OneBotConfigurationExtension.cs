using System;
using Microsoft.Extensions.DependencyInjection;

namespace OneBot.Core.Configuration;

public static class OneBotConfigurationExtension
{
    public static void AddOneBot(this IServiceCollection services, Action<OneBotBuilder> closure)
    {
        var builder = new OneBotBuilder(services);
        closure(builder);
        var cfg = builder.Build();
        services.AddSingleton<OneBotConfiguration>(cfg);
    }
}
