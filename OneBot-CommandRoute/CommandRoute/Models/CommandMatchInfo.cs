using System;
using OneBot.CommandRoute.Models.Enumeration;

namespace OneBot.CommandRoute.Models;

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
