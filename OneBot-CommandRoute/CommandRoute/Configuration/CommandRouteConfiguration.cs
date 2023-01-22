using System;
using System.Collections.Concurrent;
using OneBot.CommandRoute.Services.Implements;
using OneBot.Core.Configuration;
using OneBot.Core.Context;

namespace OneBot.CommandRoute.Configuration;

public class CommandRouteConfiguration
{

}

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

public class CommandRouteBuilder
{
    private readonly OneBotBuilder _onebot;

    private readonly CommandRouteNodeBuilder _root = new CommandRouteNodeBuilder();

    public CommandRouteBuilder(OneBotBuilder onebot)
    {
        _onebot = onebot;

    }

    public CommandRouteBuilder Route(Action<CommandRouteNodeBuilder> action)
    {
        action(_root);
        return this;
    }
}

public class CommandRouteNodeBuilder
{
    private readonly ConcurrentDictionary<string, CommandRouteNodeBuilder> _route = new ConcurrentDictionary<string, CommandRouteNodeBuilder>();

    public CommandRouteNodeBuilder Command(string name, Type controllerType, string controllerMethod)
    {
        throw new NotImplementedException();
    }

    public CommandRouteNodeBuilder Command(string name, Delegate action)
    {
        throw new NotImplementedException();
    }

    public CommandRouteNodeBuilder Fallback(Type controllerType, string controllerMethod)
    {
        throw new NotImplementedException();
    }

    public CommandRouteNodeBuilder Middleware(Type middlewareName)
    {
        throw new NotImplementedException();
    }

    public CommandRouteNodeBuilder Group(string name, Action<CommandRouteNodeBuilder> action)
    {
        var node = _route.GetOrAdd(name, new CommandRouteNodeBuilder());
        action(node);
        return this;
    }
}

public static class CommandRouteNodeBuilderExtension
{

    public static CommandRouteNodeBuilder Command<T>(this CommandRouteNodeBuilder self, string name, Func<T, Delegate> action)
    {
        self.Command(name, (OneBotContext ctx) =>
        {
            throw new NotImplementedException();
        });
        return self;
    }
}
