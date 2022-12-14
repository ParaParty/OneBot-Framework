using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneBot.Core.Interface;
using Sora.Enumeration.EventParamsType;
using Sora.EventArgs.SoraEvent;

namespace OneBot.Core.Services.Implements;

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


    public SoraHandler(IServiceProvider serviceProvider, ILogger<SoraHandler> logger, ISoraProviderService soraProviderService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _soraProviderService = soraProviderService;

        var soraService = _soraProviderService.SoraService;
        var eventManager = soraService.Event;
        eventManager.OnClientConnect += OnEvent;
        eventManager.OnClientStatusChangeEvent += OnEvent;
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

    private static List<MemberChangeType> GroupMemberIncreaseSubType = new List<MemberChangeType>() { MemberChangeType.Approve, MemberChangeType.Invite };

    private static bool GroupMemberIncreasePredictor(BaseSoraEventArgs s)
    {
        return GroupMemberIncreaseSubType.Contains(((GroupMemberChangeEventArgs)s).SubType);
    }

    private static List<MemberChangeType> GroupMemberDecreaseSubType = new List<MemberChangeType>() { MemberChangeType.Kick, MemberChangeType.KickMe, MemberChangeType.Leave };

    private static bool GroupMemberDecreasePredictor(BaseSoraEventArgs s)
    {
        return GroupMemberDecreaseSubType.Contains(((GroupMemberChangeEventArgs)s).SubType);
    }

    private Dictionary<(Type?, string?, Func<BaseSoraEventArgs, bool>?), (string, string)> _typeMap = new()
    {
        { (typeof(ConnectEventArgs), "Meta Event", null), ("meta", "connect") },
        { (typeof(ClientStatusChangeEventArgs), "Notice", null), ("meta", "connect") },
        { (typeof(EssenceChangeEventArgs), "Message", null), ("message", "group") },
        { (typeof(FileUploadEventArgs), "Message", null), ("message", "group") },
        { (typeof(FriendAddEventArgs), "Notice", null), ("notice", "friend_increase") },
        { (typeof(FriendRecallEventArgs), "Notice", null), ("notice", "private_message_delete") },
        { (typeof(FriendRequestEventArgs), "Request", null), ("notice", "qq.friend_increase_request") },    // 名字未确定
        { (typeof(GroupAdminChangeEventArgs), "Notice", null), ("notice", "qq.group_permission_changed") }, // 名字未确定
        { (typeof(GroupCardUpdateEventArgs), "Notice", null), ("notice", "qq.group_card_changed") },        // 名字未确定
        { (typeof(GroupMemberChangeEventArgs), "Notice", GroupMemberIncreasePredictor), ("notice", "group_member_increase") },
        { (typeof(GroupMemberChangeEventArgs), "Notice", GroupMemberDecreasePredictor), ("notice", "group_member_decrease") },
    };

    private ValueTask OnEvent(string eventtype, BaseSoraEventArgs eventargs)
    {
        GroupMemberChangeEventArgs t = null!;
        t.SubType.
    }
}
