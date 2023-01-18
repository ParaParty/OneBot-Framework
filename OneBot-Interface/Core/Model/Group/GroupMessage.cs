﻿using OneBot.Core.Attributes;

namespace OneBot.Core.Model.Group;

[OneBotTypeProperty("message", "group")]
public interface GroupMessage : OneBotEvent
{
    string MessageId { get; }

    Message.Message Message { get; }

    string AltMessage { get; }

    string GroupId { get; }

    string UserId { get; }
}
