using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OneBot.CommandRoute.Models.Entities;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Services.Implements;

/// <summary>
/// 事件服务
/// </summary>
public class EventService : IEventService
{
    /// <summary>
    /// 机器人服务
    /// </summary>
    private readonly IBotService _bot;

    /// <summary>`
    /// 服务容器
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Scope 工厂
    /// </summary>
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// 日志
    /// </summary>
    private ILogger<EventService> _logger;

    /// <summary>
    /// 指令路由服务
    /// </summary>
    private readonly ICommandService _commandService;

    public EventService(IBotService bot, IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory,
        ILogger<EventService> logger, ICommandService commandService)
    {
        _bot = bot;
        _serviceProvider = serviceProvider;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _commandService = commandService;
    }

    public void RegisterEventHandler()
    {
        _bot.SoraService.Event.OnClientConnect += OnGeneralEvent;
        _bot.SoraService.Event.OnGroupRequest += OnGeneralEvent;
        _bot.SoraService.Event.OnFriendRequest += OnGeneralEvent;
        _bot.SoraService.Event.OnFileUpload += OnGeneralEvent;
        _bot.SoraService.Event.OnGroupAdminChange += OnGeneralEvent;
        _bot.SoraService.Event.OnGroupMemberChange += OnGeneralEvent;
        _bot.SoraService.Event.OnGroupMemberMute += OnGeneralEvent;
        _bot.SoraService.Event.OnFriendAdd += OnGeneralEvent;
        _bot.SoraService.Event.OnGroupRecall += OnGeneralEvent;
        _bot.SoraService.Event.OnFriendRecall += OnGeneralEvent;
        _bot.SoraService.Event.OnGroupCardUpdate += OnGeneralEvent;
        _bot.SoraService.Event.OnGroupPoke += OnGeneralEvent;
        _bot.SoraService.Event.OnLuckyKingEvent += OnGeneralEvent;
        _bot.SoraService.Event.OnHonorEvent += OnGeneralEvent;
        _bot.SoraService.Event.OnTitleUpdate += OnGeneralEvent;
        _bot.SoraService.Event.OnOfflineFileEvent += OnGeneralEvent;
        _bot.SoraService.Event.OnClientStatusChangeEvent += OnGeneralEvent;
        _bot.SoraService.Event.OnEssenceChange += OnGeneralEvent;

        _bot.SoraService.Event.OnGroupMessage += OnGeneralEvent;
        _bot.SoraService.Event.OnPrivateMessage += OnGeneralEvent;

        _bot.SoraService.Event.OnSelfMessage += OnGeneralEvent;
    }

    /// <summary>
    /// 通用事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    private ValueTask OnGeneralEvent(object sender, BaseSoraEventArgs e)
    {
        using (var scope = this._scopeFactory.CreateScope())
        {
            var ctx = new OneBotContextDefault();
            ctx.SetSoraEventSender(sender);
            ctx.SetSoraEventArgs(e);
            ctx.SoraServiceScope(scope);

            var middleware = scope.ServiceProvider.GetServices<IOneBotMiddleware>().ToImmutableArray();
            var count = middleware.Count();

            OneBotRequestDelegate entry = context => _commandService.HandleEvent(context);

            for (int i = count - 1; i >= 0; i--)
            {
                var idx = i;
                var realEntry = entry;
                entry = context => middleware[idx].Invoke(context, realEntry);
            }

            Exception? exception = null;
            try
            {
                entry(ctx);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null) _commandService.EventOnException(ctx, exception);
        }

        return ValueTask.CompletedTask;
    }
}
