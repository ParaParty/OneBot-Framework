using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneBot.CommandRoute.Services;
using Sora.Entities.CQCodes;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;

namespace OneBot_CommandRoute.CommandRoute.OneBotControllers
{
    public class CQJsonRouterController: IOneBotController
    {
        /// <summary>
        /// 服务容器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 路由服务
        /// </summary>
        private readonly ICQJsonRouterService? _routeService;

        public CQJsonRouterController(ICommandService commandService, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var routeService = serviceProvider.GetService<ICQJsonRouterService>();
            if (routeService != null)
            {
                _routeService = routeService;
                commandService.Event.OnGroupMessageReceived += EventOnGroupMessageReceived;
                commandService.Event.OnPrivateMessageReceived += EventOnPrivateMessageReceived;
            }
        }

        private int EventOnGroupMessageReceived(IServiceScope scope, GroupMessageEventArgs eventArgs)
        {
            var p = eventArgs.Message.MessageList.FirstOrDefault();
            return p == null ? 0 : UniversalProcess(scope, eventArgs, p);
        }

        private int EventOnPrivateMessageReceived(IServiceScope scope, PrivateMessageEventArgs eventArgs)
        {
            var p = eventArgs.Message.MessageList.FirstOrDefault();
            return p == null ? 0 : UniversalProcess(scope, eventArgs, p);
        }

        private int UniversalProcess(IServiceScope scope, BaseSoraEventArgs eventArgs, CQCode firstElement)
        {
            var process = false;
            var appid = "";

            try
            {
                if (firstElement.Function == CQFunction.Json)
                {
                    var jsonData = ((Sora.Entities.CQCodes.CQCodeModel.Code)firstElement.CQData).Content;
                    var jObject = JObject.Parse(jsonData);

                    if (jObject.TryGetValue("app", out var jToken))
                    {
                        appid = (string) jToken;
                        process = true;
                    }
                }
            }
            catch (JsonReaderException)
            {
                // JSON 格式出错
            }

            // ReSharper disable once PossibleNullReferenceException
            return process ? _routeService.Handle(scope, eventArgs, appid) : 0;
        }
    }
}