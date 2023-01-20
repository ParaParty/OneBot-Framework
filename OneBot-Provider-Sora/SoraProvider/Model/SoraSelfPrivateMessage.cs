using Sora.EventArgs.SoraEvent;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraSelfPrivateMessage : SoraPrivateMessage
{
    public SoraSelfPrivateMessage(PrivateMessageEventArgs t) : base(t)
    {
    }
}