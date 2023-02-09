using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OneBot.Core.Context;
using OneBot.Core.Event;
using OneBot.Core.Interface;
using OneBot.Core.Model;
using OneBot.Core.Model.Message;
using OneBot.Core.Model.Message.Action;

namespace OneBot.Core.Util;

public static class OneBotContextExtension
{
    public static ValueTask<OneBotActionResponse> EmitEvent(this OneBotContext self, OneBotActionRequest args)
    {
        var emitter = self.ServiceScope.ServiceProvider.GetRequiredService<IPlatformActionEmitter>();
        return emitter.Emit(self.PlatformProviderName, args);
    }

    public static ValueTask<OneBotActionResponse> EmitEvent(this OneBotContext self, string action, IOneBotActionRequestParams args)
    {
        var emitter = self.ServiceScope.ServiceProvider.GetRequiredService<IPlatformActionEmitter>();
        return emitter.Emit(self.PlatformProviderName, action, args);
    }

    public static T? Get<T>(this OneBotEvent self, string key)
    {
        return (T?)PropertyAccessor.Get(self, key);
    }

    public static ValueTask<OneBotActionResponse> Reply(this OneBotContext self, String msg)
    {
        var mb = new Message.Builder();
        mb.Add(msg);
        return self.Reply(mb.ToMessage());
    }

    public static ValueTask<OneBotActionResponse> Reply(this OneBotContext self, Message msg)
    {
        var e = self.Event;
        var typeEntry = e.GetEventType();
        var type = typeEntry[OneBotEvent.Type.PropertyName];
        if (type != "message")
        {
            throw new InvalidCastException($"{nameof(self)} is not a message event, type:{type}");
        }
        var detailType = typeEntry[OneBotEvent.DetailType.PropertyName];
        SendMessageRequest? req = null;
        if (detailType == "private")
        {
            var userId = e.Get<string>("UserId")!;
            req = new SendMessageRequest("private", msg, userId: userId);
        }
        else if (detailType == "group")
        {
            var groupId = e.Get<string>("GroupId")!;
            req = new SendMessageRequest("private", msg, groupId: groupId);
        }
        else if (detailType == "channel")
        {
            var guildId = e.Get<string>("guildId")!;
            var channelId = e.Get<string>("ChannelId")!;
            req = new SendMessageRequest("private", msg, guildId: guildId, channelId: channelId);
        }
        else
        {
            throw new InvalidCastException($"{nameof(self)} is not a not supported message event, detailType:{detailType}");
        }
        return self.EmitEvent("send_message", req);
    }
}
