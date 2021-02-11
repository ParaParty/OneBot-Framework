using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using OneBot.CommandRoute.Laxer;
using OneBot.CommandRoute.Models.Entities;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Command
{
    /// <summary>
    /// 匹配结点
    /// </summary>
    /// <!--
    /// 假设拥有指令
    ///     CmdA SubA <... 各种参数> => 方法A
    ///     CmdA SubB <... 各种参数> => 方法B
    ///     CmdB SubC <... 各种参数> => 方法C
    ///
    /// 会被解析为
    ///     CmdA        => 无
    ///       - SubA    => 方法A
    ///       - SubB    => 方法B
    ///     CmdB        => 无方法
    ///       - SubC    => 方法C
    /// -->
    public class MatchingNode
    {
        /// <summary>
        /// 为根节点
        /// </summary>
        public bool IsRoot = false;

        /// <summary>
        /// 指令前缀
        /// </summary>
        private const string COMMAND_PREFIX = "！!/";

        /// <summary>
        /// 和这个结点相关的指令信息列表
        /// </summary>
        List<CommandModel> Command = new List<CommandModel>();

        /// <summary>
        /// 待匹配的子结点
        /// </summary>
        SortedDictionary<string, MatchingNode> Children = new SortedDictionary<string, MatchingNode>();

        /// <summary>
        /// 处理指令匹配
        /// </summary>
        /// <param name="scope">事件上下文</param>
        /// <param name="sender">事件触发者</param>
        /// <param name="e">Sora 事件对象</param>
        /// <param name="laxer">指令解析器</param>
        /// <returns>0 继续 / 1 阻断</returns>
        public int ProcessingCommandMapping(IServiceScope scope, object sender, BaseSoraEventArgs e, CommandLaxer laxer)
        {
            if (!laxer.IsValid()) return 0;

            var oldParser = laxer.Clone();

            object nextToken = null;
            try
            {
                nextToken = laxer.GetNext();
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
                    if (IsRoot && nextStep[0] >= 'A' && nextStep[0] <= 'Z')
                    {
                        if (!COMMAND_PREFIX.Contains(token[0])) continue;
                        if (nextStep != tokenUpper.Substring(1)) continue;
                    }
                    else
                    {
                        if (nextStep != tokenUpper) continue;
                    }

                    var ret = s.Value.ProcessingCommandMapping(scope, sender, e, laxer);
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

        /// <summary>
        /// 注册指令
        /// </summary>
        /// <param name="command">要注册的指令信息</param>
        /// <param name="i">深度</param>
        public void Register(CommandModel command, int i)
        {
            if (i >= command.ParametersName.Count)
            {
                // 完全匹配完力
                Command.Add(command);
                // 保证先匹配更严格的指令再匹配宽松的指令
                Command = Command.OrderByDescending(s => s.WeightA).ThenByDescending(s => s.WeightB).ToList();
                return;
            }

            if (command.ParametersMatchingType[i] != 0) {
                // 参数部分就不进入匹配了
                Command.Add(command);
                Command = Command.OrderByDescending(s => s.WeightA).ThenByDescending(s => s.WeightB).ToList();
                return;
            }

            // 挂载子结点
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