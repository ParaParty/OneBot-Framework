using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OneBot.Core.Interface;
using OneBot.Provider.SoraProvider.Model;
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
        eventManager.OnFriendAdd += OnEvent;
        eventManager.OnFriendRecall += OnEvent;
        eventManager.OnFriendRequest += OnEvent;
        eventManager.OnGroupAdminChange += OnEvent;
        eventManager.OnGroupCardUpdate += OnEvent;
        eventManager.OnGroupMemberChange += OnEvent;
        eventManager.OnGroupMemberMute += OnEvent;
        eventManager.OnGroupMessage += OnEvent;
        eventManager.OnGroupPoke += OnEvent;
        eventManager.OnGroupRecall += OnEvent;
        eventManager.OnGroupRequest += OnEvent;
        eventManager.OnHonorEvent += OnEvent;
        eventManager.OnLuckyKingEvent += OnEvent;
        eventManager.OnOfflineFileEvent += OnEvent;
        eventManager.OnPrivateMessage += OnEvent;
        eventManager.OnSelfGroupMessage += OnEvent;   // 
        eventManager.OnSelfPrivateMessage += OnEvent; //    
        eventManager.OnTitleUpdate += OnEvent;
    }

    async ValueTask OnClientConnect(string eventtype, ConnectEventArgs eventargs)
    {
        var args = new SoraConnectEventArgs(eventargs);
        await _dispatcher.Fire(args);
    }
}
