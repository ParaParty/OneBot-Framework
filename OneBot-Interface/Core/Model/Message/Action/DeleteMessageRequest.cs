namespace OneBot.Core.Model.Message.Action;

public class DeleteMessageRequest : IOneBotActionRequestParams
{
    string MessageId { get; }

    public DeleteMessageRequest(string messageId)
    {
        MessageId = messageId;
    }
}
