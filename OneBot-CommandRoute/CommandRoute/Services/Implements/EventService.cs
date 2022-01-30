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
        _bot.SoraService.Event.OnClientConnect += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.client_type", e.ClientType)
                    .AddTag("onebot.client_version_code", e.ClientVersionCode);
            });
        };
        _bot.SoraService.Event.OnGroupRequest += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.comment", e.Comment)
                    .AddTag("onebot.sender_id", e.Sender.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id)
                    .AddTag("onebot.sub_type", e.SubType);
            });
        };
        _bot.SoraService.Event.OnFriendRequest += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.comment", e.Comment)
                    .AddTag("onebot.sender_id", e.Sender.Id);
            });
        };
        _bot.SoraService.Event.OnFileUpload += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.sender_id", e.Sender.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id)
                    .AddTag("onebot.file_info.file_id", e.FileInfo.FileId)
                    .AddTag("onebot.file_info.name", e.FileInfo.Name)
                    .AddTag("onebot.file_info.size", e.FileInfo.Size)
                    .AddTag("onebot.file_info.bus_id", e.FileInfo.Busid);
            });
        };
        _bot.SoraService.Event.OnGroupAdminChange += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.sender_id", e.Sender.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id)
                    .AddTag("onebot.sub_type", e.SubType);
            });
        };
        _bot.SoraService.Event.OnGroupMemberChange += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.operator_id", e.Operator.Id)
                    .AddTag("onebot.change_user_id", e.ChangedUser.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id)
                    .AddTag("onebot.sub_type", e.SubType);
            });
        };
        _bot.SoraService.Event.OnGroupMemberMute += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.operator_id", e.Operator.Id)
                    .AddTag("onebot.user_id", e.User.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id);
            });
        };
        _bot.SoraService.Event.OnFriendAdd += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.new_friend_id", e.NewFriend.Id);
            });
        };
        _bot.SoraService.Event.OnGroupRecall += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.operator_id", e.Operator.Id)
                    .AddTag("onebot.message_sender_id", e.MessageSender.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id)
                    .AddTag("onebot.message_id", e.MessageId);
            });
        };
        _bot.SoraService.Event.OnFriendRecall += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.sender_id", e.Sender.Id)
                    .AddTag("onebot.message_id", e.MessageId);
            });
        };
        _bot.SoraService.Event.OnGroupCardUpdate += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.user_id", e.User.Id)
                    .AddTag("onebot.new_card", e.NewCard)
                    .AddTag("onebot.old_card", e.OldCard)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id);
            });
        };
        _bot.SoraService.Event.OnGroupPoke += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.send_user_id", e.SendUser.Id)
                    .AddTag("onebot.target_user_id", e.TargetUser.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id);
            });
        };
        _bot.SoraService.Event.OnLuckyKingEvent += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.send_user_id", e.SendUser.Id)
                    .AddTag("onebot.target_user_id", e.TargetUser.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id);
            });
        };
        _bot.SoraService.Event.OnHonorEvent += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.honor", e.Honor)
                    .AddTag("onebot.target_user_id", e.TargetUser.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id);
            });
        };
        _bot.SoraService.Event.OnTitleUpdate += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.new_title", e.NewTitle)
                    .AddTag("onebot.target_user_id", e.TargetUser.Id);
            });
        };
        _bot.SoraService.Event.OnOfflineFileEvent += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.sender_id", e.Sender.Id)
                    .AddTag("onebot.offline_file_info.name", e.OfflineFileInfo.Name)
                    .AddTag("onebot.offline_file_info.size", e.OfflineFileInfo.Size)
                    .AddTag("onebot.offline_file_info.url", e.OfflineFileInfo.Url);
            });
        };
        _bot.SoraService.Event.OnClientStatusChangeEvent += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.online", e.Online);
            });
        };
        _bot.SoraService.Event.OnEssenceChange += (sender, e) =>
        {
            return OnGeneralEvent(sender, e, activity =>
            {
                activity.AddTag("onebot.operator_id", e.Operator.Id)
                    .AddTag("onebot.sender_id", e.Sender.Id)
                    .AddTag("onebot.source_group_id", e.SourceGroup.Id)
                    .AddTag("onebot.message_id", e.MessageId)
                    .AddTag("onebot.essence_change_type", e.EssenceChangeType);
            });
        };

        _bot.SoraService.Event.OnGroupMessage += (sender, e) => OnGeneralEvent(sender, e, GroupMessageActivityExtensionFactory(e));
        _bot.SoraService.Event.OnPrivateMessage += (sender, e) => OnGeneralEvent(sender, e, PrivateMessageActivityExtensionFactory(e));

        _bot.SoraService.Event.OnSelfGroupMessage += (sender, e) => OnGeneralEvent(sender, e, GroupMessageActivityExtensionFactory(e));
        _bot.SoraService.Event.OnSelfPrivateMessage += (sender, e) => OnGeneralEvent(sender, e, PrivateMessageActivityExtensionFactory(e));
    }

    private Action<Activity> GroupMessageActivityExtensionFactory(GroupMessageEventArgs e)
    {
        return activity =>
        {
            activity.AddTag("onebot.is_anonymous_message", e.IsAnonymousMessage);

            if (e.Anonymous != null)
            {
                activity.AddTag("onebot.anonymous.id", e.Anonymous.Id)
                    .AddTag("onebot.anonymous.name", e.Anonymous.Name);
            }

            activity.AddTag("onebot.sender_id", e.Sender.Id)
                .AddTag("onebot.source_group_id", e.SourceGroup.Id)
                .AddTag("onebot.message", e.Message.RawText);
        };
    }

    private Action<Activity> PrivateMessageActivityExtensionFactory(PrivateMessageEventArgs e)
    {
        return activity =>
        {
            activity.AddTag("onebot.sender_id", e.Sender.Id)
                .AddTag("onebot.message", e.Message.RawText);
        };
    }

    /// <summary>
    /// 通用事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="activityExtension"></param>
    /// <returns></returns>
    private async ValueTask OnGeneralEvent(object sender, BaseSoraEventArgs e, Action<Activity> activityExtension)
    {
        using var scope = _scopeFactory.CreateScope();

        var operationName = e.EventName ?? e.GetType().FullName ?? "OneBot-Event";
        var activity = _eventActivitySource.CreateActivity(operationName, ActivityKind.Server) ?? new Activity(operationName);
        activity.Start();
        activity.AddTag("onebot.type", e.EventName);
        activity.AddTag("onebot.source_type", e.SourceType);
        activity.AddTag("onebot.login_uid", e.LoginUid);
        activityExtension(activity);

        var ctx = new OneBotContextDefault();
        ctx.SetSoraEventSender(sender);
        ctx.SetSoraEventArgs(e);
        ctx.SoraServiceScope(scope);

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
