using Sora.Interfaces;
using Sora.Net.Config;
using YukariILogService = YukariToolBox.LightLog.ILogService;
using YukariLog = YukariToolBox.LightLog.Log;

namespace OneBot.Provider.SoraProvider.Configuration;


public class SoraConfiguration
{
    public YukariILogService? Logger { get; set; } = null;

    public ISoraConfig SoraConfig { get; set; } = new ServerConfig();
}
