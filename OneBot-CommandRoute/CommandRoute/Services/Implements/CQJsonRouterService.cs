using System.Collections.Generic;
using System.Reflection;
using OneBot.CommandRoute.Attributes;
using OneBot.CommandRoute.Models;
using OneBot.CommandRoute.Models.Entities;
using OneBot.CommandRoute.Models.Enumeration;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Services.Implements;

class CQJsonRouterService : ICQJsonRouterService
{
    private readonly SortedDictionary<string, List<CQJsonRouteModel>> _mapping;

    public CQJsonRouterService()
    {
        _mapping = new();
    }

    /// <summary>
    /// 注册一个路由
    /// </summary>
    /// <param name="oneBotController"></param>
    /// <param name="method"></param>
    /// <param name="attr"></param>
    public void Register(IOneBotController oneBotController, MethodInfo method, CQJsonAttribute attr)
    {
        var mappingObj = new CQJsonRouteModel(oneBotController, method, attr);
        if (!_mapping.TryGetValue(attr.AppId, out var list))
        {
            list = new();
            _mapping[attr.AppId] = list;
        }
        list.Add(mappingObj);
    }

    /// <summary>
    /// 处理
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="eventArgs"></param>
    /// <param name="appid"></param>
    /// <returns></returns>
    public int Handle(OneBotContext scope, BaseSoraEventArgs eventArgs, string appid)
    {
        if (!_mapping.TryGetValue(appid, out var list)) return 0;
        foreach (var mappingObj in list)
        {
            switch (eventArgs)
            {
                // 检查事件类型是否正确
                case PrivateMessageEventArgs when (mappingObj.Attribute.EventType & EventType.PrivateMessage) == 0:
                case GroupMessageEventArgs when (mappingObj.Attribute.EventType & EventType.GroupMessage) == 0:
                    continue;
            }

            if (mappingObj.Invoke(scope, eventArgs) != 0) return 1;
        }
        return 0;
    }
}