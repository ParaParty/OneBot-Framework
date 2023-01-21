using System;
using System.Collections.Immutable;
namespace OneBot.Core.Configuration;

public class OneBotConfiguration
{
    public OneBotConfiguration(ImmutableArray<Type> platformProviders, ImmutableArray<Type> pipeline)
    {
        PlatformProviders = platformProviders;
        Pipeline = pipeline;
    }

    public ImmutableArray<Type> PlatformProviders { get; }

    public ImmutableArray<Type> Pipeline { get; }
}
