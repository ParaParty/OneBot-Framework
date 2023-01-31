using System;
using System.Threading.Tasks;
using OneBot.Core.Event;
using OneBot.Core.Interface;
using OneBot.Provider.SoraProvider.Configuration;
using OneBot.Provider.SoraProvider.Model;
using Sora.Enumeration.EventParamsType;
using Sora.EventArgs.SoraEvent;
using Sora.Interfaces;

namespace OneBot.Provider.SoraProvider.Services.Implementation;

public class SoraHandler
{
    private readonly IEventDispatcher _dispatcher;

    private readonly SoraConfiguration _cfg;

    private readonly string _name;

    public SoraHandler(ISoraService soraService, IEventDispatcher dispatcher, SoraConfiguration cfg)
    {
        _dispatcher = dispatcher;
        _cfg = cfg;
        _name = cfg.Name;

        var eventManager = soraService.Event;
        eventManager.OnClientConnect += OnClientConnect;
        eventManager.OnClientStatusChangeEvent += OnClientStatusChangeEvent;
        eventManager.OnEssenceChange += OnEssenceChange;
        eventManager.OnFileUpload += OnFileUpload;
        eventManager.OnFriendAdd += OnFriendAdd;
        eventManager.OnFriendRecall += OnFriendRecall;
        eventManager.OnFriendRequest += OnFriendRequest;
        eventManager.OnGroupAdminChange += OnGroupAdminChange;
        eventManager.OnGroupCardUpdate += OnGroupCardUpdate;
        eventManager.OnGroupMemberChange += OnGroupMemberChange;
        eventManager.OnGroupMemberMute += OnGroupMemberMute;
        eventManager.OnGroupMessage += OnGroupMessage;
        eventManager.OnGroupPoke += OnGroupPoke;
        eventManager.OnGroupRecall += OnGroupRecall;
        eventManager.OnGroupRequest += OnGroupRequest;
        eventManager.OnHonorEvent += OnHonorEvent;
        eventManager.OnLuckyKingEvent += OnLuckyKingEvent;
        eventManager.OnOfflineFileEvent += OnOfflineFileEvent;
        eventManager.OnPrivateMessage += OnPrivateMessage;
        eventManager.OnSelfGroupMessage += OnSelfGroupMessage;
        eventManager.OnSelfPrivateMessage += OnSelfPrivateMessage;
        eventManager.OnTitleUpdate += OnTitleUpdate;
    }

    private async ValueTask OnClientConnect(string eventType, ConnectEventArgs eventArgs)
    {
        var args = new SoraConnect(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnClientStatusChangeEvent(string eventType, ClientStatusChangeEventArgs eventArgs)
    {
        var args = new SoraClientStatusChange(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnFriendAdd(string eventType, FriendAddEventArgs eventArgs)
    {
        var args = new SoraFriendAdd(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnFriendRecall(string eventType, FriendRecallEventArgs eventArgs)
    {
        var args = new SoraFriendRecall(eventArgs);
        await _dispatcher.Dispatch(_name, args);
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
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnGroupRecall(string eventType, GroupRecallEventArgs eventArgs)
    {
        var args = new SoraGroupRecall(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnGroupMessage(string eventType, GroupMessageEventArgs eventArgs)
    {
        var args = new SoraGroupMessage(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnPrivateMessage(string eventType, PrivateMessageEventArgs eventArgs)
    {
        var args = new SoraPrivateMessage(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnEssenceChange(string eventType, EssenceChangeEventArgs eventArgs)
    {
        var args = new SoraEssenceChange(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnFileUpload(string eventType, FileUploadEventArgs eventArgs)
    {
        var args = new SoraFileUpload(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnFriendRequest(string eventType, FriendRequestEventArgs eventArgs)
    {
        var args = new SoraFriendRequest(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnGroupAdminChange(string eventType, GroupAdminChangeEventArgs eventArgs)
    {
        var args = new SoraGroupAdminChange(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnGroupCardUpdate(string eventType, GroupCardUpdateEventArgs eventArgs)
    {
        var args = new SoraGroupCardUpdate(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnGroupMemberMute(string eventType, GroupMuteEventArgs eventArgs)
    {
        var args = new SoraGroupMemberMute(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnGroupPoke(string eventType, GroupPokeEventArgs eventArgs)
    {
        var args = new SoraGroupPoke(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnGroupRequest(string eventType, AddGroupRequestEventArgs eventArgs)
    {
        var args = new SoraGroupRequest(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnHonorEvent(string eventType, HonorEventArgs eventArgs)
    {
        var args = new SoraHonorEvent(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnLuckyKingEvent(string eventType, LuckyKingEventArgs eventArgs)
    {
        var args = new SoraLuckyKingEvent(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnOfflineFileEvent(string eventType, OfflineFileEventArgs eventArgs)
    {
        var args = new SoraOfflineFileEvent(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnSelfGroupMessage(string eventType, GroupMessageEventArgs eventArgs)
    {
        var args = new SoraSelfGroupMessage(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnSelfPrivateMessage(string eventType, PrivateMessageEventArgs eventArgs)
    {
        var args = new SoraSelfPrivateMessage(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }

    private async ValueTask OnTitleUpdate(string eventType, TitleUpdateEventArgs eventArgs)
    {
        var args = new SoraTitleUpdate(eventArgs);
        await _dispatcher.Dispatch(_name, args);
    }
}
