using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneBot.CommandRoute.Services;
using Sora.Entities.Segment;
using Sora.Entities.Segment.DataModel;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.OneBotControllers
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
        private readonly ICQJsonRouterService _routeService;

#pragma warning disable 8618
        public CQJsonRouterController(ICommandService commandService, IServiceProvider serviceProvider)
#pragma warning restore 8618
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
            var p = eventArgs.Message.MessageBody.FirstOrDefault();
            return p == default ? 0 : UniversalProcess(scope, eventArgs, p);
        }

        private int EventOnPrivateMessageReceived(IServiceScope scope, PrivateMessageEventArgs eventArgs)
        {
            var p = eventArgs.Message.MessageBody.FirstOrDefault();
            return p == default ? 0 : UniversalProcess(scope, eventArgs, p);
        }

        private int UniversalProcess(IServiceScope scope, BaseSoraEventArgs eventArgs, SoraSegment firstElement)
        {
            var process = false;
            var appid = "";

            try
            {
                if (firstElement.MessageType == SegmentType.Json)
                {
                    var jsonData = ((CodeSegment)firstElement.Data).Content;
                    var jObject = JObject.Parse(jsonData);

                    if (jObject.TryGetValue("app", out var jToken))
                    {
                        // According to the signature of JObject.TryGetValue,
                        // jToken is not null when TryGetValue returns true.
#pragma warning disable 8600
                        appid = (string) jToken ?? "";
#pragma warning restore 8600
                        process = true;
                    }
                }
            }
            catch (JsonReaderException)
            {
                // JSON 格式出错
            }

            return process ? _routeService.Handle(scope, eventArgs, appid) : 0;
        }
    }
}