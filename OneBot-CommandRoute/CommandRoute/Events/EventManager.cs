using System;
using Microsoft.Extensions.DependencyInjection;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Events
{
    public class EventManager
    {
        /// <summary>客户端链接完成事件</summary>
        public event EventAsyncCallBackHandler<ConnectEventArgs>? OnClientConnect;

        /// <summary>群聊事件（触发指令之后）</summary>
        public event EventAsyncCallBackHandler<GroupMessageEventArgs>? OnGroupMessage;

        /// <summary>群聊事件（触发指令之前）</summary>
        public event EventAsyncCallBackHandler<GroupMessageEventArgs>? OnGroupMessageReceived;

        /// <summary> 登录账号发送消息事件 </summary>
        public event EventAsyncCallBackHandler<GroupMessageEventArgs>? OnSelfMessage;

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

        public delegate int EventAsyncCallBackHandler<in TEventArgs>(IServiceScope scope, TEventArgs eventArgs)
            where TEventArgs : EventArgs;

        /// <summary>
        /// 分发事件
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="eventArgs"></param>
        internal void Fire(IServiceScope scope, BaseSoraEventArgs eventArgs)
        {
            if (eventArgs is ConnectEventArgs connectEventArgs)
            {
                Fire(scope, connectEventArgs, OnClientConnect);
            }
            else if (eventArgs is GroupMessageEventArgs groupMessageEventArgs)
            {
                Fire(scope, groupMessageEventArgs, OnGroupMessage);
            }
            else if (eventArgs is PrivateMessageEventArgs privateMessageEventArgs)
            {
                Fire(scope, privateMessageEventArgs, OnPrivateMessage);
            }
            else if (eventArgs is AddGroupRequestEventArgs addGroupRequestEventArgs)
            {
                Fire(scope, addGroupRequestEventArgs, OnGroupRequest);
            }
            else if (eventArgs is FriendRequestEventArgs friendRequestEventArgs)
            {
                Fire(scope, friendRequestEventArgs, OnFriendRequest);
            }
            else if (eventArgs is FileUploadEventArgs fileUploadEventArgs)
            {
                Fire(scope, fileUploadEventArgs, OnFileUpload);
            }
            else if (eventArgs is GroupAdminChangeEventArgs groupAdminChangeEventArgs)
            {
                Fire(scope, groupAdminChangeEventArgs, OnGroupAdminChange);
            }
            else if (eventArgs is GroupMemberChangeEventArgs groupMemberChangeEventArgs)
            {
                Fire(scope, groupMemberChangeEventArgs, OnGroupMemberChange);
            }
            else if (eventArgs is GroupMuteEventArgs groupMuteEventArgs)
            {
                Fire(scope, groupMuteEventArgs, OnGroupMemberMute);
            }
            else if (eventArgs is FriendAddEventArgs friendAddEventArgs)
            {
                Fire(scope, friendAddEventArgs, OnFriendAdd);
            }
            else if (eventArgs is GroupRecallEventArgs groupRecallEventArgs)
            {
                Fire(scope, groupRecallEventArgs, OnGroupRecall);
            }
            else if (eventArgs is FriendRecallEventArgs friendRecallEventArgs)
            {
                Fire(scope, friendRecallEventArgs, OnFriendRecall);
            }
            else if (eventArgs is GroupCardUpdateEventArgs groupCardUpdateEventArgs)
            {
                Fire(scope, groupCardUpdateEventArgs, OnGroupCardUpdate);
            }
            else if (eventArgs is GroupPokeEventArgs groupPokeEventArgs)
            {
                Fire(scope, groupPokeEventArgs, OnGroupPoke);
            }
            else if (eventArgs is LuckyKingEventArgs luckyKingEventArgs)
            {
                Fire(scope, luckyKingEventArgs, OnLuckyKingEvent);
            }
            else if (eventArgs is HonorEventArgs honorEventArgs)
            {
                Fire(scope, honorEventArgs, OnHonorEvent);
            }
            else if (eventArgs is TitleUpdateEventArgs titleUpdateEventArgs)
            {
                Fire(scope, titleUpdateEventArgs, OnTitleUpdate);
            }
            else if (eventArgs is OfflineFileEventArgs offlineFileEventArgs)
            {
                Fire(scope, offlineFileEventArgs, OnOfflineFileEvent);
            }
            else if (eventArgs is ClientStatusChangeEventArgs clientStatusChangeEventArgs)
            {
                Fire(scope, clientStatusChangeEventArgs, OnClientStatusChangeEvent);
            }
            else if (eventArgs is EssenceChangeEventArgs essenceChangeEventArgs)
            {
                Fire(scope, essenceChangeEventArgs, OnEssenceChange);
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
        /// <param name="eventAsyncCallBackHandler"></param>
        /// <returns></returns>
        internal int Fire<T>(IServiceScope scope, T eventArgs, EventAsyncCallBackHandler<T>? eventAsyncCallBackHandler)
            where T : BaseSoraEventArgs
        {
            Delegate[]? listeners = eventAsyncCallBackHandler?.GetInvocationList();
            if (listeners == null) return 0;

            for (int counter = listeners.Length - 1; counter >= 0; counter--)
            {
                int ret = ((EventAsyncCallBackHandler<T>)(listeners[counter]))(scope, (T)eventArgs);
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
            return Fire(scope, privateMessageEventArgs, OnPrivateMessageReceived);
        }

        /// <summary>
        /// 分发接收到群聊消息后处理指令前事件
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="groupMessageEventArgs"></param>
        /// <returns></returns>
        internal int FireGroupMessageReceived(IServiceScope scope, GroupMessageEventArgs groupMessageEventArgs)
        {
            return Fire(scope, groupMessageEventArgs, OnGroupMessageReceived);
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
            Fire(scope, eventArgs, OnSelfMessage);
        }
    }
}
