using System;
using OneBot.CommandRoute.Services.Implements;
using OneBot.Core.Configuration;

namespace OneBot.CommandRoute.Configuration;

public static class OneBotBuilderCommandRouteExtension
{
    public static OneBotBuilder AddCommandRoute(this OneBotBuilder self, Action<CommandRouteBuilder> closure)
    {
        var services = self.Services;

        var builder = new CommandRouteBuilder(self);
        closure(builder);

        return self;
    }

    public static PipelineBuilder UseCommandRoute(this PipelineBuilder self)
    {
        return self.Use<CommandRouteMiddleware>();
    }

    public static CommandRouteBuilder AddLagencyController(this CommandRouteBuilder self)
    {
        throw new NotImplementedException();
    }
}
