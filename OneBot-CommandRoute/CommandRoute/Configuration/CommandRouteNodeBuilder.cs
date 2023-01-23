using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OneBot.CommandRoute.Models;
using OneBot.CommandRoute.Models.Enumeration;
using OneBot.Core.Interface;

namespace OneBot.CommandRoute.Configuration;

public class CommandRouteNodeBuilder
{
    private readonly ConcurrentDictionary<CommandMatchInfo, CommandRouteNodeBuilder> _route = new ConcurrentDictionary<CommandMatchInfo, CommandRouteNodeBuilder>();

    private readonly List<Type> _middleware = new List<Type>();

    private KeyValuePair<CommandMatchInfo, CommandRouteNodeBuilder>? _fallbackNode;

    private CommandDelegate? _command;

    private CommandDelegate? _fallbackCommand;

    public CommandRouteNodeBuilder Command(Type controllerType, string controllerMethodName)
    {
        var controllerMethod = controllerType.GetMethod(controllerMethodName) ??
                               throw new ArgumentException($"there is no method named {controllerMethodName} in type {controllerType.Name}");
        _command = new TypeMethodCommandDelegate(controllerType, controllerMethod);
        return this;
    }

    public CommandRouteNodeBuilder Command(Delegate action)
    {
        _command = new DelegateCommandDelegate(action);
        return this;
    }

    public CommandRouteNodeBuilder Fallback(Type controllerType, string controllerMethodName)
    {
        var controllerMethod = controllerType.GetMethod(controllerMethodName) ??
                               throw new ArgumentException($"there is no method named {controllerMethodName} in type {controllerType.Name}");
        _fallbackCommand = new TypeMethodCommandDelegate(controllerType, controllerMethod);
        return this;
    }

    public CommandRouteNodeBuilder Fallback(Type controllerType, Delegate action)
    {
        _fallbackCommand = new DelegateCommandDelegate(action);
        return this;
    }

    public CommandRouteNodeBuilder Middleware(Type middleware)
    {
        if (!middleware.IsAssignableTo(typeof(IOneBotMiddleware)))
        {
            throw new ArgumentException("middleware must be a subtype of IOneBotMiddleware");
        }
        _middleware.Add(middleware);
        return this;
    }

    internal CommandRouteNodeBuilder FallbackNode(CommandMatchInfo matchInfo, CommandRouteNodeBuilder node)
    {
        if (_fallbackNode != null)
        {
            throw new ArgumentException("command ambiguous");
        }
        _fallbackNode = new KeyValuePair<CommandMatchInfo, CommandRouteNodeBuilder>(matchInfo, node);
        return this;
    }

    public CommandRouteNodeBuilder Group(CommandMatchInfo name, Action<CommandRouteNodeBuilder> action)
    {
        action(GroupGetOrCreate(name));
        return this;
    }

    internal CommandRouteNodeBuilder GroupGetOrCreate(CommandMatchInfo name)
    {
        if (name.MatchType == CommandMatchType.OptionalParameter)
        {
            if (_fallbackNode == null )
            {
                _fallbackNode = new KeyValuePair<CommandMatchInfo, CommandRouteNodeBuilder>(name, new CommandRouteNodeBuilder());
                return _fallbackNode.Value.Value;
            }

            if (_fallbackNode.Value.Key.Name == name.Name)
            {
                return _fallbackNode.Value.Value;
            }
            
            throw new ArgumentException("command ambiguous");
        }
        else
        {
            return _route.GetOrAdd(name,_ => new CommandRouteNodeBuilder());
        }
    }
}
