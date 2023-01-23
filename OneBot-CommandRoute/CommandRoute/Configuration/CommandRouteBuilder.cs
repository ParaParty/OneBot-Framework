using System;
using OneBot.Core.Configuration;

namespace OneBot.CommandRoute.Configuration;

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
