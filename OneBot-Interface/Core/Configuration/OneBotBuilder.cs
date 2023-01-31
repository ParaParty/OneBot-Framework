using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Interface;

namespace OneBot.Core.Configuration;

public class OneBotBuilder
{
    public IServiceCollection Services { get; }

    private IDictionary<string, Type> _platformProviders = new Dictionary<string, Type>();

    private List<Type> _pipeline = new List<Type>();

    private List<Type> _preparationList = new List<Type>();

    public OneBotBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public OneBotBuilder AddPlatformProvider(string name, Type t)
    {
        if (_platformProviders.Values.Contains(t))
        {
            throw new ArgumentException($"platform provider type:{t.Name} duplicated");
        }
        if (_platformProviders.Keys.Contains(name))
        {
            throw new ArgumentException($"platform provider name:{name} duplicated");
        }
        if (!t.IsAssignableTo(typeof(IPlatformProvider)))
        {
            throw new ArgumentException("platform provider must be a subtype of IPlatformProvider");
        }

        Services.AddSingleton(t);
        Services.AddSingleton(typeof(IPlatformProvider), s => s.GetRequiredService(t));
        _platformProviders[name] =t;
        return this;
    }

    public OneBotBuilder ConfigurePipeline(Action<PipelineBuilder> closure)
    {
        var pipeline = new PipelineBuilder();
        closure(pipeline);
        _pipeline = pipeline.Build();
        return this;
    }

    public OneBotBuilder AddPreparation(Type t)
    {
        _preparationList.Add(t);
        return this;
    }

    public OneBotBuilder AddMiddleware(Type t)
    {
        if (!t.IsAssignableTo(typeof(IOneBotMiddleware)))
        {
            throw new ArgumentException("middleware must be a subtype of IOneBotMiddleware");
        }
        Services.AddSingleton(t);
        Services.AddSingleton(typeof(IOneBotMiddleware), s => s.GetRequiredService(t));
        return this;
    }

    public OneBotBuilder AddExceptionHandler(Type t)
    {
        if (!t.IsAssignableTo(typeof(IExceptionHandler)))
        {
            throw new ArgumentException("exception handler must be a subtype of IOneBotMiddleware");
        }
        Services.AddSingleton(t);
        Services.AddSingleton(typeof(IExceptionHandler), s => s.GetRequiredService(t));
        return this;
    }

    public OneBotBuilder AddHandlerResolver(Type t)
    {
        if (!t.IsAssignableTo(typeof(IHandlerResolver)))
        {
            throw new ArgumentException("handler resolver must be a subtype of IHandlerResolver");
        }
        Services.AddSingleton(t);
        Services.AddSingleton(typeof(IHandlerResolver), s => s.GetRequiredService(t));
        return this;
    }

    public OneBotBuilder AddEventHandler(Type t)
    {
        if (!t.IsAssignableTo(typeof(IEventHandler)))
        {
            throw new ArgumentException("event handler must be a subtype of IEventHandler");
        }
        Services.AddSingleton(t);
        Services.AddSingleton(typeof(IEventHandler), s => s.GetRequiredService(t));
        return this;
    }

    public OneBotBuilder AddArgumentResolver(Type t)
    {
        if (!t.IsAssignableTo(typeof(IArgumentResolver)))
        {
            throw new ArgumentException("argument resolver must be a subtype of IArgumentResolver");
        }
        Services.AddSingleton(t);
        Services.AddSingleton(typeof(IArgumentResolver), s => s.GetRequiredService(t));
        return this;
    }

    public OneBotConfiguration Build()
    {
        return new OneBotConfiguration(
            platformProviders: _platformProviders.ToImmutableDictionary(),
            pipeline: _pipeline.ToImmutableArray(),
            preparationList: _preparationList.ToImmutableArray()
        );
    }
}
