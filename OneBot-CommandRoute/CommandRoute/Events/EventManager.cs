using System;
using OneBot.CommandRoute.Models;
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

        public delegate int EventAsyncCallBackHandler<in TEventArgs>(OneBotContext scope) where TEventArgs : EventArgs;

        /// <summary>
        /// 分发事件
        /// </summary>
        /// <param name="scope"></param>
        internal void Fire(OneBotContext scope)
        {
            var eventArgs = scope.SoraEventArgs;

            if (eventArgs is ConnectEventArgs)
            {
                Fire(scope, OnClientConnect);
            }
            else if (eventArgs is GroupMessageEventArgs)
            {
                Fire(scope, OnGroupMessage);
            }
            else if (eventArgs is PrivateMessageEventArgs)
            {
                Fire(scope, OnPrivateMessage);
            }
            else if (eventArgs is AddGroupRequestEventArgs)
            {
                Fire(scope, OnGroupRequest);
            }
            else if (eventArgs is FriendRequestEventArgs)
            {
                Fire(scope, OnFriendRequest);
            }
            else if (eventArgs is FileUploadEventArgs)
            {
                Fire(scope, OnFileUpload);
            }
            else if (eventArgs is GroupAdminChangeEventArgs)
            {
                Fire(scope, OnGroupAdminChange);
            }
            else if (eventArgs is GroupMemberChangeEventArgs)
            {
                Fire(scope, OnGroupMemberChange);
            }
            else if (eventArgs is GroupMuteEventArgs)
            {
                Fire(scope, OnGroupMemberMute);
            }
            else if (eventArgs is FriendAddEventArgs)
            {
                Fire(scope, OnFriendAdd);
            }
            else if (eventArgs is GroupRecallEventArgs)
            {
                Fire(scope, OnGroupRecall);
            }
            else if (eventArgs is FriendRecallEventArgs)
            {
                Fire(scope, OnFriendRecall);
            }
            else if (eventArgs is GroupCardUpdateEventArgs)
            {
                Fire(scope, OnGroupCardUpdate);
            }
            else if (eventArgs is GroupPokeEventArgs)
            {
                Fire(scope, OnGroupPoke);
            }
            else if (eventArgs is LuckyKingEventArgs)
            {
                Fire(scope, OnLuckyKingEvent);
            }
            else if (eventArgs is HonorEventArgs)
            {
                Fire(scope, OnHonorEvent);
            }
            else if (eventArgs is TitleUpdateEventArgs)
            {
                Fire(scope, OnTitleUpdate);
            }
            else if (eventArgs is OfflineFileEventArgs)
            {
                Fire(scope, OnOfflineFileEvent);
            }
            else if (eventArgs is ClientStatusChangeEventArgs)
            {
                Fire(scope, OnClientStatusChangeEvent);
            }
            else if (eventArgs is EssenceChangeEventArgs)
            {
                Fire(scope, OnEssenceChange);
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
        /// <param name="eventAsyncCallBackHandler"></param>
        /// <returns></returns>
        private int Fire<T>(OneBotContext scope, EventAsyncCallBackHandler<T>? eventAsyncCallBackHandler)
            where T : BaseSoraEventArgs
        {
            Delegate[]? listeners = eventAsyncCallBackHandler?.GetInvocationList();
            if (listeners == null) return 0;

            for (int counter = listeners.Length - 1; counter >= 0; counter--)
            {
                int ret = ((EventAsyncCallBackHandler<T>)listeners[counter])(scope);
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
            if (OnException == null) return false;
            OnException?.Invoke(scope, exception);
            return true;
        }

        /// <summary>
        /// 分发登录账号发送消息事件
        /// </summary>
        /// <param name="scope"></param>
        internal void FireSelfMessage(OneBotContext scope)
        {
            Fire(scope, OnSelfMessage);
        }
    }
}
