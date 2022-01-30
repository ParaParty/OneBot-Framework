using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
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
    // ReSharper disable once NotAccessedField.Local
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Scope 工厂
    /// </summary>
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// 日志
    /// </summary>
    // ReSharper disable once NotAccessedField.Local
    private ILogger<EventService> _logger;

    /// <summary>
    /// 指令路由服务
    /// </summary>
    private readonly ICommandService _commandService;

    /// <summary>
    /// 事件源
    /// </summary>
    private readonly ActivitySource _eventActivitySource = new ActivitySource("OneBot.Event", Assembly.GetExecutingAssembly().GetName().Version!.ToString());

    /// <summary>
    /// 中间件源
    /// </summary>
    private readonly ActivitySource _middlewareActivitySource = new ActivitySource("OneBot.Middleware", Assembly.GetExecutingAssembly().GetName().Version!.ToString());
    
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

        _bot.SoraService.Event.OnSelfGroupMessage += OnGeneralEvent;
        _bot.SoraService.Event.OnSelfPrivateMessage += OnGeneralEvent;
    }

    /// <summary>
    /// 通用事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    private async ValueTask OnGeneralEvent(object sender, BaseSoraEventArgs e)
    {
        using var scope = _scopeFactory.CreateScope();

        var ctx = new OneBotContextDefault();
        ctx.SetSoraEventSender(sender);
        ctx.SetSoraEventArgs(e);
        ctx.SoraServiceScope(scope);
        
        var operationName = e.EventName ?? e.GetType().FullName ?? "OneBot-Event";
        var activity = _eventActivitySource.CreateActivity(operationName, ActivityKind.Server) ?? new Activity(operationName);
        activity.AddTag("onebot:type", e.EventName);
        activity.Start();

        var ctxHolder = scope.ServiceProvider.GetService<IOneBotContextHolder>();
        if (ctxHolder is OneBotContextHolder holder)
        {
            holder.SetOneBotContext(ctx);
        }
        
        var middleware = scope.ServiceProvider.GetServices<IOneBotMiddleware>().ToImmutableArray();
        var count = middleware.Length;

        OneBotRequestDelegate entry = async context =>
        {
            const string middleOperationName = "OneBot.CommandRoute";
            var middlewareActivity = _middlewareActivitySource.CreateActivity(middleOperationName, ActivityKind.Server) ?? new Activity(middleOperationName);
            middlewareActivity.Start();
            await _commandService.HandleEvent(context);
            middlewareActivity.Stop();
        };

        for (int i = count - 1; i >= 0; i--)
        {
            var idx = i;
            var realEntry = entry;
            entry = async context =>
            {
                var middleOperationName = middleware[idx].GetType().FullName ?? "?";
                var middlewareActivity = _middlewareActivitySource.CreateActivity(middleOperationName, ActivityKind.Server) ?? new Activity(middleOperationName);
                middlewareActivity.Start();
                await middleware[idx].Invoke(context, realEntry);
                middlewareActivity.Stop();
            };
        }

        try
        {
            await entry(ctx);
        }
        catch (Exception ex)
        {
            await _commandService.EventOnException(ctx, ex);
        }
        
        activity.Stop();
    }
}
