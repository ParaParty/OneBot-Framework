using System;
using Microsoft.Extensions.DependencyInjection;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Events
{
    public class EventManager
    {
        /// <summary>客户端链接完成事件</summary>
        public event EventManager.EventAsyncCallBackHandler<ConnectEventArgs>? OnClientConnect;

        /// <summary>群聊事件（触发指令之后）</summary>
        public event EventManager.EventAsyncCallBackHandler<GroupMessageEventArgs>? OnGroupMessage;

        /// <summary>群聊事件（触发指令之前）</summary>
        public event EventManager.EventAsyncCallBackHandler<GroupMessageEventArgs>? OnGroupMessageReceived;

        /// <summary> 登录账号发送消息事件 </summary>
        public event EventAsyncCallBackHandler<GroupMessageEventArgs>? OnSelfMessage;

        /// <summary>私聊事件（触发指令之后）</summary>
        public event EventManager.EventAsyncCallBackHandler<PrivateMessageEventArgs>? OnPrivateMessage;

        /// <summary>私聊事件（触发指令之前）</summary>
        public event EventManager.EventAsyncCallBackHandler<PrivateMessageEventArgs>? OnPrivateMessageReceived;

        /// <summary>群申请事件</summary>
        public event EventManager.EventAsyncCallBackHandler<AddGroupRequestEventArgs>? OnGroupRequest;

        /// <summary>好友申请事件</summary>
        public event EventManager.EventAsyncCallBackHandler<FriendRequestEventArgs>? OnFriendRequest;

        /// <summary>群文件上传事件</summary>
        public event EventManager.EventAsyncCallBackHandler<FileUploadEventArgs>? OnFileUpload;

        /// <summary>管理员变动事件</summary>
        public event EventManager.EventAsyncCallBackHandler<GroupAdminChangeEventArgs>? OnGroupAdminChange;

        /// <summary>群成员变动事件</summary>
        public event EventManager.EventAsyncCallBackHandler<GroupMemberChangeEventArgs>? OnGroupMemberChange;

        /// <summary>群成员禁言事件</summary>
        public event EventManager.EventAsyncCallBackHandler<GroupMuteEventArgs>? OnGroupMemberMute;

        /// <summary>好友添加事件</summary>
        public event EventManager.EventAsyncCallBackHandler<FriendAddEventArgs>? OnFriendAdd;

        /// <summary>群聊撤回事件</summary>
        public event EventManager.EventAsyncCallBackHandler<GroupRecallEventArgs>? OnGroupRecall;

        /// <summary>好友撤回事件</summary>
        public event EventManager.EventAsyncCallBackHandler<FriendRecallEventArgs>? OnFriendRecall;

        /// <summary>群名片变更事件</summary>
        public event EventManager.EventAsyncCallBackHandler<GroupCardUpdateEventArgs>? OnGroupCardUpdate;

        /// <summary>群内戳一戳事件</summary>
        public event EventManager.EventAsyncCallBackHandler<GroupPokeEventArgs>? OnGroupPoke;

        /// <summary>运气王事件</summary>
        public event EventManager.EventAsyncCallBackHandler<LuckyKingEventArgs>? OnLuckyKingEvent;

        /// <summary>群成员荣誉变更事件</summary>
        public event EventManager.EventAsyncCallBackHandler<HonorEventArgs>? OnHonorEvent;

        /// <summary>离线文件事件</summary>
        public event EventManager.EventAsyncCallBackHandler<OfflineFileEventArgs>? OnOfflineFileEvent;

        /// <summary>其他客户端在线状态变更事件</summary>
        public event EventManager.EventAsyncCallBackHandler<ClientStatusChangeEventArgs>? OnClientStatusChangeEvent;

        /// <summary>其他客户端在线状态变更事件</summary>
        public event EventManager.EventAsyncCallBackHandler<EssenceChangeEventArgs>? OnEssenceChange;

        public delegate int EventAsyncCallBackHandler<in TEventArgs>(
            IServiceScope scope,
            TEventArgs eventArgs)
            where TEventArgs : System.EventArgs;

        /// <summary>
        /// 分发事件
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="eventArgs"></param>
        internal void Fire(IServiceScope scope, BaseSoraEventArgs eventArgs)
        {
            if (eventArgs is ConnectEventArgs)
            {
                Fire<ConnectEventArgs>(scope, eventArgs, OnClientConnect?.GetInvocationList());
            }
            else if (eventArgs is GroupMessageEventArgs)
            {
                Fire<GroupMessageEventArgs>(scope, eventArgs, OnGroupMessage?.GetInvocationList());
            }
            else if (eventArgs is PrivateMessageEventArgs)
            {
                Fire<PrivateMessageEventArgs>(scope, eventArgs, OnPrivateMessage?.GetInvocationList());
            }
            else if (eventArgs is AddGroupRequestEventArgs)
            {
                Fire<AddGroupRequestEventArgs>(scope, eventArgs, OnGroupRequest?.GetInvocationList());
            }
            else if (eventArgs is FriendRequestEventArgs)
            {
                Fire<FriendRequestEventArgs>(scope, eventArgs, OnFriendRequest?.GetInvocationList());
            }
            else if (eventArgs is FileUploadEventArgs)
            {
                Fire<FileUploadEventArgs>(scope, eventArgs, OnFileUpload?.GetInvocationList());
            }
            else if (eventArgs is GroupAdminChangeEventArgs)
            {
                Fire<GroupAdminChangeEventArgs>(scope, eventArgs, OnGroupAdminChange?.GetInvocationList());
            }
            else if (eventArgs is GroupMemberChangeEventArgs)
            {
                Fire<GroupMemberChangeEventArgs>(scope, eventArgs, OnGroupMemberChange?.GetInvocationList());
            }
            else if (eventArgs is GroupMuteEventArgs)
            {
                Fire<GroupMuteEventArgs>(scope, eventArgs, OnGroupMemberMute?.GetInvocationList());
            }
            else if (eventArgs is FriendAddEventArgs)
            {
                Fire<FriendAddEventArgs>(scope, eventArgs, OnFriendAdd?.GetInvocationList());
            }
            else if (eventArgs is GroupRecallEventArgs)
            {
                Fire<GroupRecallEventArgs>(scope, eventArgs, OnGroupRecall?.GetInvocationList());
            }
            else if (eventArgs is FriendRecallEventArgs)
            {
                Fire<FriendRecallEventArgs>(scope, eventArgs, OnFriendRecall?.GetInvocationList());
            }
            else if (eventArgs is GroupCardUpdateEventArgs)
            {
                Fire<GroupCardUpdateEventArgs>(scope, eventArgs, OnGroupCardUpdate?.GetInvocationList());
            }
            else if (eventArgs is GroupPokeEventArgs)
            {
                Fire<GroupPokeEventArgs>(scope, eventArgs, OnGroupPoke?.GetInvocationList());
            }
            else if (eventArgs is LuckyKingEventArgs)
            {
                Fire<LuckyKingEventArgs>(scope, eventArgs, OnLuckyKingEvent?.GetInvocationList());
            }
            else if (eventArgs is HonorEventArgs)
            {
                Fire<HonorEventArgs>(scope, eventArgs, OnHonorEvent?.GetInvocationList());
            }
            else if (eventArgs is OfflineFileEventArgs)
            {
                Fire<OfflineFileEventArgs>(scope, eventArgs, OnOfflineFileEvent?.GetInvocationList());
            }
            else if (eventArgs is ClientStatusChangeEventArgs)
            {
                Fire<ClientStatusChangeEventArgs>(scope, eventArgs, OnClientStatusChangeEvent?.GetInvocationList());
            }
            else if (eventArgs is EssenceChangeEventArgs)
            {
                Fire<EssenceChangeEventArgs>(scope, eventArgs, OnEssenceChange?.GetInvocationList());
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
        /// <param name="scope"></param>
        /// <param name="eventArgs"></param>
        /// <param name="listeners"></param>
        /// <returns></returns>
        internal int Fire<T>(IServiceScope scope, BaseSoraEventArgs eventArgs, Delegate[]? listeners)
            where T : BaseSoraEventArgs
        {
            if (listeners == null) return 0;

            for (int counter = listeners.Length - 1; counter >= 0; counter--)
            {
                int ret = ((EventAsyncCallBackHandler<T>) (listeners[counter]))(scope, (T) eventArgs);
                if (ret != 0) return ret;
            }

            return 0;
        }

        /// <summary>
        /// 分发接收到私聊消息后处理指令前事件
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="privateMessageEventArgs"></param>
        /// <returns></returns>
        internal int FirePrivateMessageReceived(IServiceScope scope, PrivateMessageEventArgs privateMessageEventArgs)
        {
            return Fire<PrivateMessageEventArgs>(scope, privateMessageEventArgs,
                OnPrivateMessageReceived?.GetInvocationList());
        }

        /// <summary>
        /// 分发接收到群聊消息后处理指令前事件
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="groupMessageEventArgs"></param>
        /// <returns></returns>
        internal int FireGroupMessageReceived(IServiceScope scope, GroupMessageEventArgs groupMessageEventArgs)
        {
            return Fire<GroupMessageEventArgs>(scope, groupMessageEventArgs,
                OnGroupMessageReceived?.GetInvocationList());
        }

        /// <summary>
        /// 异常处理事件
        /// </summary>
        public event ExceptionDelegate? OnException;

        public delegate void ExceptionDelegate(IServiceScope scope, BaseSoraEventArgs e, Exception exception);

        /// <summary>
        /// 分发异常
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="e"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        internal bool FireException(IServiceScope scope, BaseSoraEventArgs e, Exception exception)
        {
            if (OnException == null) return false;

            OnException?.Invoke(scope, e, exception);
            return true;
        }

        /// <summary>
        /// 分发登录账号发送消息事件
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="eventArgs"></param>
        internal void FireSelfMessage(IServiceScope scope, GroupMessageEventArgs eventArgs)
        {
            Fire<EssenceChangeEventArgs>(scope, eventArgs, OnSelfMessage?.GetInvocationList());
        }
    }
}