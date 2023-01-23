using System;
using System.Collections.Generic;
using System.Linq;
using OneBot.CommandRoute.Models;
using OneBot.CommandRoute.Models.Enumeration;
using OneBot.CommandRoute.Util;
using OneBot.Core.Context;

namespace OneBot.CommandRoute.Configuration;

public static class CommandRouteNodeBuilderExtension
{
    public static CommandRouteNodeBuilder Command<T>(this CommandRouteNodeBuilder self, string name, Func<T, Delegate> action)
    {
        var wrapper = new HandlerDelegateWrapper<T>(action);
        self.Command(name, async (OneBotContext ctx) => await wrapper.Invoke(ctx));
        return self;
    }

    internal static CommandRouteNodeBuilder Walk(this CommandRouteNodeBuilder self, CommandMatchInfo matchType)
    {
        return self.GroupGetOrCreate(matchType);
    }

    internal static CommandRouteNodeBuilder Walk(this CommandRouteNodeBuilder self, IEnumerable<CommandMatchInfo> matchType)
    {
        var ret = self;
        foreach (var item in matchType)
        {
            ret = ret.GroupGetOrCreate(item);
        }
        return ret;
    }

    public static void Command(this CommandRouteNodeBuilder self, CommandMatchInfo matchType, Type controllerType, string controllerMethod)
    {
        var node = self.Walk(matchType);
        node.Command(controllerType, controllerMethod);
    }

    public static void Command(this CommandRouteNodeBuilder self, CommandMatchInfo matchType, Delegate action)
    {
        var node = self.Walk(matchType);
        node.Command(action);
    }

    public static void Command(this CommandRouteNodeBuilder self, IEnumerable<CommandMatchInfo> matchType, Type controllerType, string controllerMethod)
    {
        var node = self.Walk(matchType);
        node.Command(controllerType, controllerMethod);
    }

    public static void Command(this CommandRouteNodeBuilder self, IEnumerable<CommandMatchInfo> matchType, Delegate action)
    {
        var node = self.Walk(matchType);
        node.Command(action);
    }

    public static void Command(this CommandRouteNodeBuilder self, string pattern, Type controllerType, string controllerMethod)
    {
        var step = ParsePattern(pattern);
        self.Command(step, controllerType, controllerMethod);
    }

    public static void Command(this CommandRouteNodeBuilder self, string pattern, Delegate action)
    {
        var step = ParsePattern(pattern);
        self.Command(step, action);
    }

    public static CommandRouteNodeBuilder Group(this CommandRouteNodeBuilder self, string pattern, Action<CommandRouteNodeBuilder> action)
    {
        var step = ParsePattern(pattern);
        var node = self.Walk(step);
        action(node);
        return self;
    }

    private static IEnumerable<CommandMatchInfo> ParsePattern(string pattern)
    {
        return ParsePattern(pattern.Trim().Split(' ').Select(s => s.Trim()).ToList());
    }

    private static IEnumerable<CommandMatchInfo> ParsePattern(List<string> pattern)
    {
        // 参数匹配类型
        List<CommandMatchType> parametersMatchingType = new List<CommandMatchType>();

        // 被全字匹配的字符串或参数名
        List<string> parametersName = new List<string>();

        // 从指令定义中解析出 参数匹配类型 和 参数匹配字符串或参数名
        var withOptional = false;
        foreach (var s in pattern)
        {
            // 必选参数
            if (s.StartsWith("<") && s.EndsWith(">"))
            {
                if (withOptional)
                {
                    throw new ArgumentException("optional parameters must at the end of command");
                }

                parametersMatchingType.Add(CommandMatchType.RequiredParameter);
                var paraName = s.Substring(1, s.Length - 2);
                for (var j = 0; j < parametersName.Count; j++)
                {
                    if (parametersMatchingType[j] == 0) continue;
                    if (parametersName[j] == paraName)
                    {
                        throw new ArgumentException("parameters must be unique");
                    }
                }

                parametersName.Add(paraName);
                continue;
            }

            // 可选参数
            if (s.StartsWith("[") && s.EndsWith("]"))
            {
                withOptional = true;
                parametersMatchingType.Add(CommandMatchType.OptionalParameter);
                var paraName = s.Substring(1, s.Length - 2);
                for (var j = 0; j < parametersName.Count; j++)
                {
                    if (parametersMatchingType[j] == 0) continue;
                    if (parametersName[j] == paraName)
                    {
                        throw new ArgumentException("parameters must be unique");
                    }
                }

                parametersName.Add(paraName);
                continue;
            }

            // 全字匹配
            if (withOptional)
            {
                throw new ArgumentException("optional parameters must at the end of command");
            }

            parametersMatchingType.Add(CommandMatchType.Literal);
            parametersName.Add(s);
            continue;
        }

        if (parametersName.Count != parametersMatchingType.Count || parametersMatchingType.Count != pattern.Count)
        {
            throw new ArgumentException("unexpected exception happened");
        }

        var ret = new List<CommandMatchInfo>();
        for (int i = 0; i < parametersName.Count; i++)
        {
            ret.Add(new CommandMatchInfo(parametersMatchingType[i], parametersName[i]));
        }
        return ret;
    }
}
