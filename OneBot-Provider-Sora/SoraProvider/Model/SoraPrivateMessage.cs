using OneBot.Core.Event;
using OneBot.Core.Model.Message;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraPrivateMessage : PrivateMessage, UnderlaySoraEvent<PrivateMessageEventArgs>
{
    public SoraPrivateMessage(PrivateMessageEventArgs t)
    {
        WrappedModel = t;
        Message = t.Message.ConvertToOneBotMessage();
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public PrivateMessageEventArgs WrappedModel { get; init; }

    public string MessageId => WrappedModel.Message.MessageId.ToString();

    public Message Message { get; }

    public string AltMessage => WrappedModel.Message.RawText;


    public string UserId => WrappedModel.Sender.Id.ToString();

    public Message GetMessage()
    {
        return Message;
    }
}
