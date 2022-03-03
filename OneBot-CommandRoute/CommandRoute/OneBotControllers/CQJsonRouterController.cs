using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneBot.CommandRoute.Models;
using OneBot.CommandRoute.Services;
using Sora.Entities.Segment;
using Sora.Entities.Segment.DataModel;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.OneBotControllers;

public class CQJsonRouterController : IOneBotController
{
    /// <summary>
    /// 路由服务
    /// </summary>
    private readonly ICQJsonRouterService _routeService;

    public CQJsonRouterController(ICommandService commandService, IServiceProvider serviceProvider)
    {
        _routeService = null!;
        var routeService = serviceProvider.GetService<ICQJsonRouterService>();
        if (routeService != null)
        {
            _routeService = routeService;
            commandService.Event.OnGroupMessageReceived += EventOnGroupMessageReceived;
            commandService.Event.OnPrivateMessageReceived += EventOnPrivateMessageReceived;
        }
    }

    private int EventOnGroupMessageReceived(OneBotContext scope)
    {
        var eventArgs = scope.WrapSoraEventArgs<GroupMessageEventArgs>();
        var p = eventArgs.Message.MessageBody.FirstOrDefault(p => p.MessageType == SegmentType.Json);
        return p == default ? 0 : UniversalProcess(scope, eventArgs, p);
    }

    private int EventOnPrivateMessageReceived(OneBotContext scope)
    {
        var eventArgs = scope.WrapSoraEventArgs<PrivateMessageEventArgs>();
        var p = eventArgs.Message.MessageBody.FirstOrDefault(p => p.MessageType == SegmentType.Json);
        return p == default ? 0 : UniversalProcess(scope, eventArgs, p);
    }

    private int UniversalProcess(OneBotContext scope, BaseSoraEventArgs eventArgs, SoraSegment firstElement)
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
                    appid = jToken.Value<string>() ?? "";
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