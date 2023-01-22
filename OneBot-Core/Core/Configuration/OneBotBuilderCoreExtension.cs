using System;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Interface;
using OneBot.Core.Resolvers.Arguments;
using OneBot.Core.Services;

namespace OneBot.Core.Configuration;

public static class OneBotBuilderCoreExtension
{
    public static OneBotBuilder AddOneBotCore(this OneBotBuilder self, Action<OneBotCoreBuilder> closure)
    {
        var services = self.Services;

        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        services.AddSingleton<IOneBotContextHolder, OneBotContextHolder>();
        
        services.AddSingleton<IOneBotService, OneBotService>();
        services.AddSingleton<IPlatformProviderManager, PlatformProviderManager>();
        services.AddSingleton<IInitializationManager, InitializationManager>();
        
        services.AddSingleton<IExceptionHandlerManager, ExceptionHandlerManager>();
        services.AddSingleton<IHandlerInvokeTool, HandlerInvokeTool>();
        services.AddSingleton<IHandlerManager, HandlerManager>();
        services.AddSingleton<IPipelineManager, PipelineManager>();

        self.AddHandlerResolver<IHandlerResolver>();
        self.AddMiddleware<EventHandlerMiddleware>();

        self.AddArgumentResolver<OneBotCtxResolver>();
        self.AddArgumentResolver<FromServiceResolver>();

        closure(new OneBotCoreBuilder(self));

        return self;
    }

    public static OneBotBuilder AddOneBotCore(this OneBotBuilder self)
    {
        return self.AddOneBotCore(_ => { });
    }

    public static PipelineBuilder UseEventHandler(this PipelineBuilder self)
    {
        return self.Use<EventHandlerMiddleware>();
    }
}
