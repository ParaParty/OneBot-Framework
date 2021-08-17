using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using OneBot.CommandRoute.Lexer;
using OneBot.CommandRoute.Models.Entities;
using OneBot.CommandRoute.Services;
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
        private readonly IOneBotCommandRouteConfiguration _configuration;

        /// <summary>
        /// 和这个结点相关的指令信息列表
        /// </summary>
        private List<CommandModel> _command = new List<CommandModel>();

        /// <summary>
        /// 待匹配的子结点
        /// </summary>
        private readonly SortedDictionary<string, MatchingNode> _children = new SortedDictionary<string, MatchingNode>();

        public MatchingNode(IOneBotCommandRouteConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// 处理指令匹配
        /// </summary>
        /// <param name="scope">事件上下文</param>
        /// <param name="sender">事件触发者</param>
        /// <param name="e">Sora 事件对象</param>
        /// <param name="lexer">指令解析器</param>
        /// <returns>0 继续 / 1 阻断</returns>
        public int ProcessingCommandMapping(IServiceScope scope, object sender, BaseSoraEventArgs e, CommandLexer lexer)
        {
            if (!lexer.IsValid()) return 0;

            var oldParser = lexer.Clone();

            object? nextToken = null;
            try
            {
                nextToken = lexer.GetNext();
            }
            catch (ParserToTheEndException)
            {
            }

            if (nextToken is string token)
            {
                foreach (var s in _children)
                {
                    var nextStepForComparing = (_configuration.IsCaseSensitive) ? s.Key : s.Key.ToUpper();
                    var tokenForComparing = (_configuration.IsCaseSensitive) ? token : token.ToUpper();
                    
                    // 如果是根，并且有设置指令前缀，并且是英文指令，那么我们就处理指令前缀匹配
                    if (IsRoot && _configuration.CommandPrefix.Length > 0 && nextStepForComparing[0] >= 'A' && nextStepForComparing[0] <= 'Z')
                    {
                        if (!_configuration.CommandPrefix.Contains("" + token[0])) continue;
                        if (nextStepForComparing != tokenForComparing[1..]) continue;
                    }
                    else
                    {
                        if (nextStepForComparing != tokenForComparing) continue;
                    }

                    var ret = s.Value.ProcessingCommandMapping(scope, sender, e, lexer);
                    if (ret != 0) return ret;
                }
            }

            foreach (var s in _command)
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
                _command.Add(command);
                // 保证先匹配更严格的指令再匹配宽松的指令
                _command = _command.OrderByDescending(s => s.WeightA).ThenByDescending(s => s.WeightB).ToList();
                return;
            }

            if (command.ParametersMatchingType[i] != 0)
            {
                // 参数部分就不进入匹配了
                _command.Add(command);
                _command = _command.OrderByDescending(s => s.WeightA).ThenByDescending(s => s.WeightB).ToList();
                return;
            }

            // 挂载子结点
            if (!_children.TryGetValue(command.ParametersName[i], out var tmp))
            {
                tmp = new MatchingNode(_configuration);
                _children[command.ParametersName[i]] = tmp;
            }

            tmp.Register(command, i + 1);
        }
    }
}