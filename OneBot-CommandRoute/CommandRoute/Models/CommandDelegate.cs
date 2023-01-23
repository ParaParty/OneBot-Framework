using System;
using System.Reflection;
using OneBot.CommandRoute.Models.Enumeration;

namespace OneBot.CommandRoute.Models;

public class CommandDelegate
{

}
public class TypeMethodCommandDelegate: CommandDelegate
{
    private readonly Type _type;

    private readonly MethodInfo _method;

    public TypeMethodCommandDelegate(Type type, MethodInfo method)
    {
        _type = type;
        _method = method;
    }
}

public class DelegateCommandDelegate: CommandDelegate
{
    private readonly Delegate _action;

    public DelegateCommandDelegate(Delegate action)
    {
        _action = action;
    }
}

public struct CommandMatchInfo
{
    public CommandMatchInfo(CommandMatchType matchType, string name)
    {
        MatchType = matchType;
        Name = name;
    }

    public CommandMatchType MatchType { get; init; }

    public String Name { get; init; }
}
