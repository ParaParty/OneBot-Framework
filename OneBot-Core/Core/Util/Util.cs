using System.Reflection;

namespace OneBot.Core.Util;

internal static class Common
{
    internal static string Version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
}
