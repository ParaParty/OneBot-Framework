using System.Collections.Generic;
using OneBot.Core.Model.Meta;
using OneBot.Provider.SoraProvider.Util;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraClientStatusChange : StatusUpdate, UnderlaySoraEvent<ClientStatusChangeEventArgs>
{
    public SoraClientStatusChange(ClientStatusChangeEventArgs e)
    {
        WrappedModel = e;
        var bots = new List<StatusUpdate.Bot>()
        {
            new SoraBot(new SoraBot.SelfModel(e.LoginUid), e.Online)
        };
        Status = new SoraStatusModel(bots);
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string SubType => "";

    public StatusUpdate.StatusModel Status { get; }

    public ClientStatusChangeEventArgs WrappedModel { get; }

    public class SoraStatusModel : StatusUpdate.StatusModel
    {
        public SoraStatusModel(List<StatusUpdate.Bot> bots)
        {
            Bots = bots;
        }

        public bool Good => true;

        public List<StatusUpdate.Bot> Bots { get; }
    }

    public class SoraBot : StatusUpdate.Bot
    {
        public SoraBot(SelfModel self, bool online)
        {
            Self = self;
            Online = online;
        }

        public object Self { get; }

        public bool Online { get; }

        public class SelfModel
        {
            public SelfModel(long userId)
            {
                UserId = userId.ToString();
            }

            string UserId { get; }
        }
    }
}
