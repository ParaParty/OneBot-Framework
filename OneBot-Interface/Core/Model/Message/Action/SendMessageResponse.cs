using System;

namespace OneBot.Core.Model.Message.Action;

public interface SendMessageResponse
{
    string MessageId { get; }

    Int64 Time { get; }
}
