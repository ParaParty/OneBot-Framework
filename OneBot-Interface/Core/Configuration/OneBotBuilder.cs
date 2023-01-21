using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Interface;

namespace OneBot.Core.Configuration;

public class OneBotBuilder
{
    public IServiceCollection Services { get; }

    private List<Type> _platformProviders = new List<Type>();

    private List<Type> _pipeline = new List<Type>();

    public OneBotBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public OneBotBuilder AddPlatformProvider<T>() where T : class, IPlatformProvider
    {
        if (_platformProviders.Contains(typeof(T)))
        {
            throw new ArgumentException($"platform provider {typeof(T).Name} duplicated");
        }

        Services.AddSingleton<IPlatformProvider, T>();
        _platformProviders.Add(typeof(T));
        return this;
    }

    public OneBotBuilder ConfigurePipeline(Action<PipelineBuilder> closure)
    {
        var pipeline = new PipelineBuilder();
        closure(pipeline);
        _pipeline = pipeline.Build();
        return this;
    }

    public OneBotConfiguration Build()
    {
        return new OneBotConfiguration(_platformProviders.ToImmutableArray(), _pipeline.ToImmutableArray());
    }
}
