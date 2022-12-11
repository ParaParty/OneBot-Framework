using System;
using System.Diagnostics;
using System.Reflection;
using OneBot.CommandRoute.Models;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Events;

public class EventManager
{
    /// <summary>客户端链接完成事件</summary>
    public event EventAsyncCallBackHandler<ConnectEventArgs>? OnClientConnect;

    /// <summary>群聊事件（触发指令之后）</summary>
    public event EventAsyncCallBackHandler<GroupMessageEventArgs>? OnGroupMessage;

    /// <summary>群聊事件（触发指令之前）</summary>
    public event EventAsyncCallBackHandler<GroupMessageEventArgs>? OnGroupMessageReceived;

    /// <summary> 登录账号发送消息事件（群聊） </summary>
    public event EventAsyncCallBackHandler<GroupMessageEventArgs>? OnSelfGroupMessage;
        
    /// <summary> 登录账号发送消息事件（私聊） </summary>
    public event EventAsyncCallBackHandler<GroupMessageEventArgs>? OnSelfPrivateMessage;

    /// <summary>私聊事件（触发指令之后）</summary>
    public event EventAsyncCallBackHandler<PrivateMessageEventArgs>? OnPrivateMessage;

    /// <summary>私聊事件（触发指令之前）</summary>
    public event EventAsyncCallBackHandler<PrivateMessageEventArgs>? OnPrivateMessageReceived;

    /// <summary>群申请事件</summary>
    public event EventAsyncCallBackHandler<AddGroupRequestEventArgs>? OnGroupRequest;

    /// <summary>好友申请事件</summary>
    public event EventAsyncCallBackHandler<FriendRequestEventArgs>? OnFriendRequest;

    /// <summary>群文件上传事件</summary>
    public event EventAsyncCallBackHandler<FileUploadEventArgs>? OnFileUpload;

    /// <summary>管理员变动事件</summary>
    public event EventAsyncCallBackHandler<GroupAdminChangeEventArgs>? OnGroupAdminChange;

    /// <summary>群成员变动事件</summary>
    public event EventAsyncCallBackHandler<GroupMemberChangeEventArgs>? OnGroupMemberChange;

    /// <summary>群成员禁言事件</summary>
    public event EventAsyncCallBackHandler<GroupMuteEventArgs>? OnGroupMemberMute;

    /// <summary>好友添加事件</summary>
    public event EventAsyncCallBackHandler<FriendAddEventArgs>? OnFriendAdd;

    /// <summary>群聊撤回事件</summary>
    public event EventAsyncCallBackHandler<GroupRecallEventArgs>? OnGroupRecall;

    /// <summary>好友撤回事件</summary>
    public event EventAsyncCallBackHandler<FriendRecallEventArgs>? OnFriendRecall;

    /// <summary>群名片变更事件</summary>
    public event EventAsyncCallBackHandler<GroupCardUpdateEventArgs>? OnGroupCardUpdate;

    /// <summary>群内戳一戳事件</summary>
    public event EventAsyncCallBackHandler<GroupPokeEventArgs>? OnGroupPoke;

    /// <summary>运气王事件</summary>
    public event EventAsyncCallBackHandler<LuckyKingEventArgs>? OnLuckyKingEvent;

    /// <summary>群成员荣誉变更事件</summary>
    public event EventAsyncCallBackHandler<HonorEventArgs>? OnHonorEvent;

    /// <summary>群成员头衔更新</summary>
    public event EventAsyncCallBackHandler<TitleUpdateEventArgs>? OnTitleUpdate;

    /// <summary>离线文件事件</summary>
    public event EventAsyncCallBackHandler<OfflineFileEventArgs>? OnOfflineFileEvent;

    /// <summary>其他客户端在线状态变更事件</summary>
    public event EventAsyncCallBackHandler<ClientStatusChangeEventArgs>? OnClientStatusChangeEvent;

    /// <summary>其他客户端在线状态变更事件</summary>
    public event EventAsyncCallBackHandler<EssenceChangeEventArgs>? OnEssenceChange;

    // ReSharper disable once UnusedTypeParameter
    public delegate int EventAsyncCallBackHandler<in TEventArgs>(OneBotContext scope) where TEventArgs : EventArgs;

    /// <summary>
    /// 事件源
    /// </summary>
    private static readonly ActivitySource EventRouteActivitySource = new ActivitySource("OneBot.EventRoute", Assembly.GetExecutingAssembly().GetName().Version!.ToString());

    /// <summary>
    /// 分发事件
    /// </summary>
    /// <param name="context"></param>
    internal void Fire(OneBotContext context)
    {
        var eventArgs = context.SoraEventArgs;

        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (eventArgs is ConnectEventArgs)
        {
            Fire(context, OnClientConnect);
        }
        else if (eventArgs is GroupMessageEventArgs)
        {
            Fire(context, OnGroupMessage);
        }
        else if (eventArgs is PrivateMessageEventArgs)
        {
            Fire(context, OnPrivateMessage);
        }
        else if (eventArgs is AddGroupRequestEventArgs)
        {
            Fire(context, OnGroupRequest);
        }
        else if (eventArgs is FriendRequestEventArgs)
        {
            Fire(context, OnFriendRequest);
        }
        else if (eventArgs is FileUploadEventArgs)
        {
            Fire(context, OnFileUpload);
        }
        else if (eventArgs is GroupAdminChangeEventArgs)
        {
            Fire(context, OnGroupAdminChange);
        }
        else if (eventArgs is GroupMemberChangeEventArgs)
        {
            Fire(context, OnGroupMemberChange);
        }
        else if (eventArgs is GroupMuteEventArgs)
        {
            Fire(context, OnGroupMemberMute);
        }
        else if (eventArgs is FriendAddEventArgs)
        {
            Fire(context, OnFriendAdd);
        }
        else if (eventArgs is GroupRecallEventArgs)
        {
            Fire(context, OnGroupRecall);
        }
        else if (eventArgs is FriendRecallEventArgs)
        {
            Fire(context, OnFriendRecall);
        }
        else if (eventArgs is GroupCardUpdateEventArgs)
        {
            Fire(context, OnGroupCardUpdate);
        }
        else if (eventArgs is GroupPokeEventArgs)
        {
            Fire(context, OnGroupPoke);
        }
        else if (eventArgs is LuckyKingEventArgs)
        {
            Fire(context, OnLuckyKingEvent);
        }
        else if (eventArgs is HonorEventArgs)
        {
            Fire(context, OnHonorEvent);
        }
        else if (eventArgs is TitleUpdateEventArgs)
        {
            Fire(context, OnTitleUpdate);
        }
        else if (eventArgs is OfflineFileEventArgs)
        {
            Fire(context, OnOfflineFileEvent);
        }
        else if (eventArgs is ClientStatusChangeEventArgs)
        {
            Fire(context, OnClientStatusChangeEvent);
        }
        else if (eventArgs is EssenceChangeEventArgs)
        {
            Fire(context, OnEssenceChange);
        }
        else
        {
            throw new EventHandleException("不存在这样的事件。");
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="eventAsyncCallBackHandler"></param>
    /// <returns></returns>
    // ReSharper disable once MemberCanBeMadeStatic.Local
    private int Fire<T>(OneBotContext context, EventAsyncCallBackHandler<T>? eventAsyncCallBackHandler)
        where T : BaseSoraEventArgs
    {
        Delegate[]? listeners = eventAsyncCallBackHandler?.GetInvocationList();
        if (listeners == null) return 0;

        for (int counter = listeners.Length - 1; counter >= 0; counter--)
        {
            var func = (EventAsyncCallBackHandler<T>)listeners[counter];
            var target = func.Target;
            var method = func.Method;
            var operationName = (target?.GetType().FullName ?? "?") + "::" + method.Name;
            var activity = EventRouteActivitySource.CreateActivity(operationName, ActivityKind.Internal) ?? new Activity(operationName);
            activity.Start();
            int ret = func(context);
            activity.Stop();

            if (ret != 0) return ret;
        }

        return 0;
    }

    /// <summary>
    /// 分发接收到私聊消息后处理指令前事件
    /// </summary>
    /// <param name="scope"></param>
    /// <returns></returns>
    internal int FirePrivateMessageReceived(OneBotContext scope)
    {
        return Fire(scope, OnPrivateMessageReceived);
    }

    /// <summary>
    /// 分发接收到群聊消息后处理指令前事件
    /// </summary>
    /// <param name="scope"></param>
    /// <returns></returns>
    internal int FireGroupMessageReceived(OneBotContext scope)
    {
        return Fire(scope, OnGroupMessageReceived);
    }

    /// <summary>
    /// 异常处理事件
    /// </summary>
    public event ExceptionDelegate? OnException;

    public delegate void ExceptionDelegate(OneBotContext scope, Exception exception);

    /// <summary>
    /// 分发异常
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    internal bool FireException(OneBotContext scope, Exception exception)
    {
        var onException = OnException;
        if (onException == null) return false;
        onException(scope, exception);
        return true;
    }

    /// <summary>
    /// 分发登录账号发送消息事件（群聊）
    /// </summary>
    /// <param name="scope"></param>
    internal void FireSelfGroupMessage(OneBotContext scope)
    {
        Fire(scope, OnSelfGroupMessage);
    }
        
    /// <summary>
    /// 分发登录账号发送消息事件（私聊）
    /// </summary>
    /// <param name="scope"></param>
    internal void FireSelfPrivateMessage(OneBotContext scope)
    {
        Fire(scope, OnSelfPrivateMessage);
    }
}