using System;

namespace OneBot.Core.Model.Message.Action;

public class SendMessageResponse : IOneBotActionResponseData
{
    public SendMessageResponse(string messageId, long time)
    {
        MessageId = messageId;
        Time = time;
    }

    string MessageId { get; }

    Int64 Time { get; }
}
