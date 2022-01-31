// ReSharper disable once CheckNamespace
namespace OpenTelemetry.Trace;

public static class TraceProviderBuilderExtensions
{
    public static TracerProviderBuilder AddOneBotInstrumentation(
        this TracerProviderBuilder builder)
    {
        builder.AddSource("OneBot.Event", "OneBot.Middleware", "OneBot.CommandRoute", "OneBot.EventRoute");
        return builder;
    }
}
