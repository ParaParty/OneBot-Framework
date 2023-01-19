using OneBot.Core.Model;
using Sora.EventArgs.SoraEvent;

namespace OneBot.Provider.SoraProvider.Model;

public interface UnderlaySoraEvent<T> : UnderlayModel<T> where T : BaseSoraEventArgs
{

}
