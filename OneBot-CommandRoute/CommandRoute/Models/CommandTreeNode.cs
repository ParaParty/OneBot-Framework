using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace OneBot.CommandRoute.Models;

public class CommandTreeNode
{
    public ImmutableDictionary<CommandMatchInfo, CommandTreeNode> Route { get; }

    public ImmutableList<Type> Middleware { get; }

    public KeyValuePair<CommandMatchInfo, CommandTreeNode>? FallbackNode { get; }

    public CommandDelegate? Command { get; }
    
    public CommandTreeNode(IDictionary<CommandMatchInfo, CommandTreeNode> route, List<Type> middleware, KeyValuePair<CommandMatchInfo, CommandTreeNode>? fallbackNode, CommandDelegate? command)
    {
        Route = route.ToImmutableDictionary();
        Middleware = middleware.ToImmutableList();
        FallbackNode = fallbackNode;
        Command = command;
    }
}
