using System;
using OneBot.Core.Resolvers.Arguments;

namespace OneBot.Core.Configuration;

public class OneBotCoreBuilder
{
    private readonly OneBotBuilder _onebotBuilder;

    public OneBotCoreBuilder(OneBotBuilder onebotBuilder)
    {
        _onebotBuilder = onebotBuilder;
    }

    [Obsolete("may make argument resolving ambiguous")]
    public OneBotCoreBuilder AddArgumentDependencyInjectionResolver()
    {
        _onebotBuilder.AddArgumentResolver<DependencyInjectionResolver>();
        return this;
    }
}
