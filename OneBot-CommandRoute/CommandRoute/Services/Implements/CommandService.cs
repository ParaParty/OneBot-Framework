using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OneBot.CommandRoute.Attributes;
using OneBot.CommandRoute.Command;
using OneBot.CommandRoute.Events;
using OneBot.CommandRoute.Laxer;
using OneBot.CommandRoute.Models.Entities;
using Sora.EventArgs.SoraEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OneBot.CommandRoute.Services.Implements
{
    /// <summary>
    /// 指令路由服务
    /// </summary>
    public class CommandService : ICommandService
    {
        /// <summary>
        /// CQHTTP 服务
        /// </summary>
        private IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 指令匹配树根节点
        /// </summary>
        private readonly MatchingNode _matchingRootNode = new MatchingNode();

        /// <summary>
        /// Scope 工厂
        /// </summary>
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<CommandService> _logger;

        /// <summary>
        /// 事件中心
        /// </summary>
        public EventManager Event { get; set; }

        public CommandService(IBotService bot, IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory,
            ILogger<CommandService> logger)
        {
            ServiceProvider = serviceProvider;
            _scopeFactory = scopeFactory;
            _logger = logger;
            Event = new EventManager();
            _matchingRootNode.IsRoot = true;

            bot.Server.Event.OnClientConnect += OnGeneralEvent;
            bot.Server.Event.OnGroupRequest += OnGeneralEvent;
            bot.Server.Event.OnFriendRequest += OnGeneralEvent;
            bot.Server.Event.OnFileUpload += OnGeneralEvent;
            bot.Server.Event.OnGroupAdminChange += OnGeneralEvent;
            bot.Server.Event.OnGroupMemberChange += OnGeneralEvent;
            bot.Server.Event.OnGroupMemberMute += OnGeneralEvent;
            bot.Server.Event.OnFriendAdd += OnGeneralEvent;
            bot.Server.Event.OnGroupRecall += OnGeneralEvent;
            bot.Server.Event.OnFriendRecall += OnGeneralEvent;
            bot.Server.Event.OnGroupCardUpdate += OnGeneralEvent;
            bot.Server.Event.OnGroupPoke += OnGeneralEvent;
            bot.Server.Event.OnLuckyKingEvent += OnGeneralEvent;
            bot.Server.Event.OnHonorEvent += OnGeneralEvent;
            bot.Server.Event.OnOfflineFileEvent += OnGeneralEvent;
            bot.Server.Event.OnClientStatusChangeEvent += OnGeneralEvent;
            bot.Server.Event.OnEssenceChange += OnGeneralEvent;

            bot.Server.Event.OnGroupMessage += EventOnGroupMessage;
            bot.Server.Event.OnPrivateMessage += EventOnPrivateMessage;

            bot.Server.Event.OnSelfMessage += EventOnSelfMessage;
        }

        #region 事件处理

        /// <summary>
        /// 登录账号发送消息事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventargs"></param>
        /// <returns></returns>
        private ValueTask EventOnSelfMessage(string type, GroupMessageEventArgs e)
        {
            using var scope = this._scopeFactory.CreateScope();
            Event.FireSelfMessage(scope, e);
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="e"></param>
        /// <param name="exception"></param>
        private void EventOnException(IServiceScope scope, BaseSoraEventArgs e, Exception exception)
        {
            try
            {
                if (!Event.FireException(scope, e, exception))
                {
                    _logger.LogError(
                        $"{exception.Message} : \n" +
                        $"{exception.StackTrace}"
                    );
                }

                ;
            }
            catch (Exception e1)
            {
                _logger.LogError(
                    $"{exception.Message} : \n" +
                    $"{exception.StackTrace}"
                );

                _logger.LogError(
                    $"{e1.Message} : \n" +
                    $"{e1.StackTrace}"
                );
            }
        }

        /// <summary>
        /// 通用事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private ValueTask OnGeneralEvent(object sender, BaseSoraEventArgs e)
        {
            return OnGeneralEvent(sender, e, null);
        }

        /// <summary>
        /// 通用事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        private ValueTask OnGeneralEvent(object sender, BaseSoraEventArgs e, IServiceScope scope)
        {
            if (scope == null) scope = this._scopeFactory.CreateScope();

            Exception exception = null;
            try
            {
                Event.Fire(scope, e);
            }
            catch (Exception e1)
            {
                exception = e1;
            }

            if (exception != null) EventOnException(scope, e, exception);

            return ValueTask.CompletedTask;
        }

        #endregion 事件处理

        #region 指令路由

        /// <summary>
        /// 私聊消息分发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private ValueTask EventOnPrivateMessage(object sender, PrivateMessageEventArgs e)
        {
            using var scope = this._scopeFactory.CreateScope();

            Exception exception = null;
            try
            {
                if (Event.FirePrivateMessageReceived(scope, e) != 0) return ValueTask.CompletedTask;
                if (_matchingRootNode.ProcessingCommandMapping(scope, sender, e,
                    new CommandLaxer(e.Message.MessageList)) != 0) return ValueTask.CompletedTask;
            }
            catch (Exception e1)
            {
                exception = e1;
            }

            if (exception != null) EventOnException(scope, e, exception);

            OnGeneralEvent(sender, e, scope);
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// 群聊消息分发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private ValueTask EventOnGroupMessage(object sender, GroupMessageEventArgs e)
        {
            using var scope = this._scopeFactory.CreateScope();

            Exception exception = null;
            try
            {
                if (Event.FireGroupMessageReceived(scope, e) != 0) return ValueTask.CompletedTask;
                if (_matchingRootNode.ProcessingCommandMapping(scope, sender, e,
                    new CommandLaxer(e.Message.MessageList)) != 0) return ValueTask.CompletedTask;
            }
            catch (Exception e1)
            {
                exception = e1;
            }

            if (exception != null) EventOnException(scope, e, exception);


            OnGeneralEvent(sender, e, scope);
            return ValueTask.CompletedTask;
        }

        #endregion 指令路由

        #region 注册指令

        /// <summary>
        /// 注册指令
        /// </summary>
        public void RegisterCommand()
        {
            var onebotController = ServiceProvider.GetServices<IOneBotController>();
            foreach (var s in onebotController)
            {
                var clazz = s.GetType();
                var methods = clazz.GetMethods();
                foreach (var method in methods)
                {
                    var methodAttributes = method.CustomAttributes;
                    if (System.Attribute.IsDefined(method, typeof(CommandAttribute)))
                    {
                        var attr = (CommandAttribute) System.Attribute.GetCustomAttribute(method,
                            typeof(CommandAttribute));
                        RegisterCommand(s, method, attr);
                    }

                    ;
                }
            }
        }

        private void RegisterCommand(IOneBotController commandObj, MethodInfo commandMethod, CommandAttribute attribute)
        {
            List<string> command = attribute.Pattern.Trim().Split(' ').ToList();
            List<string> aliasList = attribute.Alias.Trim().Split(',').Select(s => s.Trim()).Where(s => s.Length > 0)
                .ToList();

            try
            {
                RegisterCommand(commandObj, commandMethod, command, attribute);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(
                    $"{e.Message} : \n" +
                    $"{e.StackTrace}"
                );
            }

            foreach (var c in aliasList.Select(s => s.Split(' ').ToList()))
            {
                try
                {
                    RegisterCommand(commandObj, commandMethod, c, attribute);
                }
                catch (ArgumentException e)
                {
                    _logger.LogError(
                        $"{e.Message} : \n" +
                        $"{e.StackTrace}"
                    );
                }
            }
        }

        private void RegisterCommand(IOneBotController commandObj, MethodInfo commandMethod, List<string> matchPattern,
            CommandAttribute attribute)
        {
            // 参数类型
            List<Type> parametersType = new List<Type>();

            // 参数匹配类型：0全字匹配 1参数 2可选参数
            List<int> parametersMatchingType = new List<int>();

            // 被全字匹配的字符串或参数名
            List<string> parametersName = new List<string>();

            // 这个参数是否被用过了
            List<bool> parametersUsed = new List<bool>();

            // 这个参数对应函数的形参列表哪一位。负数表示不映射。
            List<int> parameterPositionMapping = new List<int>();

            // 从指令定义中解析出 参数匹配类型 和 参数匹配字符串或参数名
            var withOptional = false;
            foreach (var s in matchPattern)
            {
                parametersType.Add(typeof(string));
                parametersUsed.Add(false);
                parameterPositionMapping.Add(-1);

                // 必选参数
                if (s.StartsWith("<") && s.EndsWith(">"))
                {
                    if (withOptional)
                    {
                        throw new ArgumentException($"我觉得你的指令定义有问题。可选参数只能写在末尾哦。");
                    }

                    parametersMatchingType.Add(1);
                    var paraName = s.Substring(1, s.Length - 2);
                    for (var j = 0; j < parametersName.Count; j++)
                    {
                        if (parametersMatchingType[j] == 0) continue;
                        if (parametersName[j] == paraName)
                        {
                            throw new ArgumentException($"我觉得你的指令定义有问题。参数名必须互异。");
                        }
                    }

                    parametersName.Add(paraName);
                    continue;
                }

                // 可选参数
                if (s.StartsWith("[") && s.EndsWith("]"))
                {
                    withOptional = true;
                    parametersMatchingType.Add(2);
                    var paraName = s.Substring(1, s.Length - 2);
                    for (var j = 0; j < parametersName.Count; j++)
                    {
                        if (parametersMatchingType[j] == 0) continue;
                        if (parametersName[j] == paraName)
                        {
                            throw new ArgumentException($"我觉得你的指令定义有问题。参数名必须互异。");
                        }
                    }

                    parametersName.Add(paraName);
                    continue;
                }

                // 全字匹配
                if (withOptional)
                {
                    throw new ArgumentException($"我觉得你的指令定义有问题。可选参数只能写在末尾哦。");
                }

                parametersMatchingType.Add(0);
                parametersName.Add(s);
                continue;
            }

            ;

            // 先检查属性
            var functionParametersList = commandMethod.GetParameters();
            for (var i = 0; i < functionParametersList.Length; i++)
            {
                var type = functionParametersList[i];
                if (System.Attribute.IsDefined(type, typeof(CommandParameterAttribute)))
                {
                    var attr = (CommandParameterAttribute) System.Attribute.GetCustomAttribute(type,
                        typeof(CommandParameterAttribute));
                    var paraName = attr.Name;

                    var idx = -1;
                    for (var j = 0; j < parametersName.Count; j++)
                    {
                        if (parametersMatchingType[j] == 0) continue;
                        if (parametersName[j] != paraName) continue;

                        idx = j;
                        break;
                    }

                    if (idx < 0)
                    {
                        throw new ArgumentException($"我觉得你的指令定义有问题。在指令定义中找不到参数名为 {paraName} 的参数。");
                    }

                    parametersType[idx] = type.ParameterType;
                    parametersUsed[idx] = true;
                    parameterPositionMapping[idx] = i;
                }
            }


            // 再检查形参参数名
            for (var i = 0; i < parametersName.Count; i++)
            {
                if (parametersUsed[i]) continue;
                if (parametersMatchingType[i] == 0) continue;

                var paraName = parametersName[i];

                for (var j = 0; j < functionParametersList.Length; j++)
                {
                    var type = functionParametersList[j];
                    if (type.Name == paraName)
                    {
                        parametersType[i] = type.ParameterType;
                        parametersUsed[i] = true;
                        parameterPositionMapping[i] = j;
                    }
                }
            }

            _matchingRootNode.Register(
                new CommandModel(commandObj, commandMethod,
                    parametersType, parametersMatchingType, parametersName, parameterPositionMapping,
                    attribute)
                , 0);
        }

        #endregion 注册指令
    }
}