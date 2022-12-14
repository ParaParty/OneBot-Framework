using System;

namespace OneBot.CommandRoute.Model.Action;

public interface SendMessageResponse
{
    string MessageId { get; }
    Int64 Time { get; }
}
