using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using QQRobot.Services;
using Sora.EventArgs.SoraEvent;

namespace QQRobot.CommandRoute.MatchingNode
{
    public class MatchingNode
    {
        private const string COMMAND_PREFIX = "!/";


        List<CommandModel> Command = new List<CommandModel>();

        SortedDictionary<String, MatchingNode> Children = new SortedDictionary<string, MatchingNode>();

        public int ProcessingCommandMapping(IServiceScope scope, object sender, BaseSoraEventArgs e, CommandParser parser)
        {
            if (!parser.IsValid()) return 0;

            var oldParser = parser.Clone();

            object nextToken = null;
            try
            {
                nextToken = parser.GetNext();
            }
            catch (ParserToTheEndException)
            {
                
            }
            if (nextToken is string token)
            {
                foreach (var s in Children)
                {
                    var nextStep = s.Key.ToUpper();
                    var tokenUpper = token.ToUpper();
                    if (nextStep[0] >= 'A' && nextStep[0] <= 'Z')
                    {
                        if (!COMMAND_PREFIX.Contains(token[0])) continue;
                        if (nextStep != tokenUpper.Substring(1)) continue;
                    }
                    else
                    {
                        if (nextStep != tokenUpper) continue;
                    }

                    var ret = s.Value.ProcessingCommandMapping(scope, sender, e, parser);
                    if (ret != 0) return ret;
                }
            }

            foreach (var s in Command)
            {
                var ret = s.Invoke(scope, sender, e, oldParser);
                if (ret != 0) return ret;
            }
            return 0;
        }

        public void Register(CommandModel command, int i)
        {
            if (i >= command.ParametersName.Count)
            {
                // 完全匹配完力
                Command.Add(command);
                Command = Command.OrderByDescending(s => s.WeightA).ThenByDescending(s => s.WeightB).ToList();
                return;
            }

            if (command.ParametersMatchingType[i] != 0) {
                // 参数部分就不进入匹配了
                Command.Add(command);
                Command = Command.OrderByDescending(s => s.WeightA).ThenByDescending(s => s.WeightB).ToList();
                return;
            }

            MatchingNode tmp;
            if (!Children.TryGetValue(command.ParametersName[i], out tmp))
            {
                tmp = new MatchingNode();
                Children[command.ParametersName[i]] = tmp;
            }
            tmp.Register(command, i + 1);

        }
    }
}