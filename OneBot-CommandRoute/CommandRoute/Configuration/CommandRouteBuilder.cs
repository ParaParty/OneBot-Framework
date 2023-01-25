using System;
using System.Collections.Generic;
using System.Linq;
using OneBot.Core.Configuration;

namespace OneBot.CommandRoute.Configuration;

public class CommandRouteBuilder
{
    private readonly OneBotBuilder _onebot;

    private readonly CommandRouteNodeBuilder _root = new CommandRouteNodeBuilder();

    private string[] _commandPrefix = { "！", "!", "/" };

    private bool _caseSensitive = true;

    public CommandRouteBuilder(OneBotBuilder onebot)
    {
        _onebot = onebot;

    }

    public CommandRouteBuilder Route(Action<CommandRouteNodeBuilder> action)
    {
        action(_root);
        return this;
    }

    public CommandRouteBuilder SetCommandPrefix(IEnumerable<string> prefix)
    {
        _commandPrefix = prefix.ToArray();
        return this;
    }

    public CommandRouteBuilder SetCaseSensitive(bool enable)
    {
        _caseSensitive = enable;
        return this;
    }

    public CommandRouteConfiguration Build()
    {
        var ret = new CommandRouteConfiguration(
            commandTreeRoot: _root.Build(),
            commandPrefix: _commandPrefix,
            caseSensitive: _caseSensitive
        );
        return ret;
    }
}
