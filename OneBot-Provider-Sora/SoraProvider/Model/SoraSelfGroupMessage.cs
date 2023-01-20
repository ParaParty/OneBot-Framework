using Sora.EventArgs.SoraEvent;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraSelfGroupMessage : SoraGroupMessage
{
    public SoraSelfGroupMessage(GroupMessageEventArgs t) : base(t)
    {
    }
}