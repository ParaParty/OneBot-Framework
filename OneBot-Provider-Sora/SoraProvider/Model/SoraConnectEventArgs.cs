using OneBot.Core.Model.Meta;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraConnectEventArgs : Connect, UnderlaySoraEvent<ConnectEventArgs>
{
    public SoraConnectEventArgs(ConnectEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
        version = new ConnVersion("", "", "11");
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string Type => "meta";

    public string DetailType => "connect";

    public string SubType => "";

    public Connect.Version version { get; }

    public ConnectEventArgs WrappedModel { get; init; }

    class ConnVersion : Connect.Version
    {

        public ConnVersion(string impl, string version, string onebotVersion)
        {
            Impl = impl;
            Version = version;
            OnebotVersion = onebotVersion;
        }

        public string Impl { get; init; }

        public string Version { get; init; }

        public string OnebotVersion { get; init; }
    }

}
