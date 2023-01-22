using OneBot.Core.Interface;

namespace OneBot.Core.Configuration;

public static class PipelineBuilderExtension
{
    public static PipelineBuilder Use<T>(this PipelineBuilder self) where T : class, IOneBotMiddleware
    {
        return self.Use(typeof(T));
    }
}
