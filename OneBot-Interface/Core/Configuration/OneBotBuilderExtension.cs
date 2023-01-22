using OneBot.Core.Interface;

namespace OneBot.Core.Configuration;

public static class OneBotBuilderExtension
{
    public static OneBotBuilder AddMiddleware<T>(this OneBotBuilder self) where T : class, IOneBotMiddleware
    {
        return self.AddMiddleware(typeof(T));
    }

    public static OneBotBuilder AddExceptionHandler<T>(this OneBotBuilder self) where T : class, IExceptionHandler
    {
        return self.AddExceptionHandler(typeof(T));
    }

    public static OneBotBuilder AddHandlerResolver<T>(this OneBotBuilder self) where T : class, IHandlerResolver
    {
        return self.AddHandlerResolver(typeof(T));
    }

    public static OneBotBuilder AddEventHandler<T>(this OneBotBuilder self) where T : class, IEventHandler
    {
        return self.AddEventHandler(typeof(T));
    }
    
    public static OneBotBuilder AddArgumentResolver<T>(this OneBotBuilder self) where T : class, IArgumentResolver
    {
        return self.AddArgumentResolver(typeof(T));
    }
}
