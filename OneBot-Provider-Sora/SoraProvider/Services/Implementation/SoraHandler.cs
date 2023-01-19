using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OneBot.Core.Interface;
using OneBot.Core.Model;
using OneBot.Provider.SoraProvider.Model;
using Sora.Enumeration.EventParamsType;
using Sora.EventArgs.SoraEvent;

namespace OneBot.Provider.SoraProvider.Services.Implementation;

public class SoraHandler : IAdapterHandler
{
    /// <summary>
    /// 服务容器
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger<SoraHandler> _logger;

    private readonly ISoraProviderService _soraProviderService;

    private readonly IOneBotEventDispatcher _dispatcher;


    public SoraHandler(IServiceProvider serviceProvider, ILogger<SoraHandler> logger, ISoraProviderService soraProviderService, IOneBotEventDispatcher dispatcher)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _soraProviderService = soraProviderService;
        _dispatcher = dispatcher;

        var soraService = _soraProviderService.SoraService;
        var eventManager = soraService.Event;
        eventManager.OnClientConnect += OnClientConnect;
        eventManager.OnClientStatusChangeEvent += OnClientStatusChangeEvent;
        eventManager.OnEssenceChange += OnEvent;
        eventManager.OnFileUpload += OnEvent;
        eventManager.OnFriendAdd += OnFriendAdd;
        eventManager.OnFriendRecall += OnFriendRecall;
        eventManager.OnFriendRequest += OnEvent;
        eventManager.OnGroupAdminChange += OnEvent;
        eventManager.OnGroupCardUpdate += OnEvent;
        eventManager.OnGroupMemberChange += OnGroupMemberChange;
        eventManager.OnGroupMemberMute += OnEvent;
        eventManager.OnGroupMessage += OnGroupMessage;
        eventManager.OnGroupPoke += OnEvent;
        eventManager.OnGroupRecall += OnGroupRecall;
        eventManager.OnGroupRequest += OnEvent;
        eventManager.OnHonorEvent += OnEvent;
        eventManager.OnLuckyKingEvent += OnEvent;
        eventManager.OnOfflineFileEvent += OnEvent;
        eventManager.OnPrivateMessage += OnPrivateMessage;
        eventManager.OnSelfGroupMessage += OnEvent;
        eventManager.OnSelfPrivateMessage += OnEvent;
        eventManager.OnTitleUpdate += OnEvent;
    }

    private async ValueTask OnClientConnect(string eventType, ConnectEventArgs eventArgs)
    {
        var args = new SoraConnectEventArgs(eventArgs);
        await _dispatcher.Fire(args);
    }

    private async ValueTask OnClientStatusChangeEvent(string eventType, ClientStatusChangeEventArgs eventArgs)
    {
        var args = new SoraClientStatusChangeEventArgs(eventArgs);
        await _dispatcher.Fire(args);
    }

    private async ValueTask OnFriendAdd(string eventType, FriendAddEventArgs eventArgs)
    {
        var args = new SoraFriendAdd(eventArgs);
        await _dispatcher.Fire(args);
    }

    private async ValueTask OnFriendRecall(string eventType, FriendRecallEventArgs eventArgs)
    {
        var args = new SoraFriendRecall(eventArgs);
        await _dispatcher.Fire(args);
    }

    private async ValueTask OnGroupMemberChange(string eventType, GroupMemberChangeEventArgs eventArgs)
    {
        OneBotEvent args = eventArgs.SubType switch
        {

            MemberChangeType.Unknown => throw new ArgumentException("eventArgs.SubType not acceptable"),
            MemberChangeType.Leave => new SoraGroupMemberDecrease(eventArgs),
            MemberChangeType.Kick => new SoraGroupMemberDecrease(eventArgs),
            MemberChangeType.KickMe => new SoraGroupMemberDecrease(eventArgs),
            MemberChangeType.Approve => new SoraGroupMemberIncrease(eventArgs),
            MemberChangeType.Invite => new SoraGroupMemberIncrease(eventArgs),
            _ => throw new ArgumentOutOfRangeException()
        };
        await _dispatcher.Fire(args);
    }
    
    private async ValueTask OnGroupRecall(string eventType, GroupRecallEventArgs eventArgs)
    {
        var args = new SoraGroupRecall(eventArgs);
        await _dispatcher.Fire(args);
    }  
    
    private async ValueTask OnGroupMessage(string eventType, GroupMessageEventArgs eventArgs)
    {
        var args = new SoraGroupMessageEventArgs(eventArgs);
        await _dispatcher.Fire(args);
    }
    
    private async ValueTask OnPrivateMessage(string eventType, PrivateMessageEventArgs eventArgs)
    {
        var args = new SoraPrivateMessageEventArgs(eventArgs);
        await _dispatcher.Fire(args);
    }
}
