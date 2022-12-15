using OneBot.Core.Model.Meta;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider.Model;

public class SoraConnectEventArgs : Connect, UnderlaySoraEvent<ConnectEventArgs>
{
    public SoraConnectEventArgs(ConnectEventArgs wrappedModel)
    {
        WrappedModel = wrappedModel;
        Version = new SoraConnVersion("", "", "11");
    }

    public string Id => WrappedModel.GenerateId();

    public double Time => WrappedModel.Time.ToTimeStamp();

    public string Type => "meta";

    public string DetailType => "connect";

    public string SubType => "";

    public Connect.ConnVersion Version { get; }

    public ConnectEventArgs WrappedModel { get; init; }

    class SoraConnVersion : Connect.ConnVersion
    {

        public SoraConnVersion(string impl, string version, string onebotVersion)
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
