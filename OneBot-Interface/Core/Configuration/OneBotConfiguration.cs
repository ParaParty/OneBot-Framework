using System;
using System.Collections.Immutable;

namespace OneBot.Core.Configuration;

public class OneBotConfiguration
{
    public OneBotConfiguration(ImmutableArray<Type> platformProviders, ImmutableArray<Type> pipeline, ImmutableArray<Type> preparationList)
    {
        PlatformProviders = platformProviders;
        Pipeline = pipeline;
        PreparationList = preparationList;
    }

    public ImmutableArray<Type> PlatformProviders { get; }

    public ImmutableArray<Type> Pipeline { get; }

    public ImmutableArray<Type> PreparationList { get; }
}
