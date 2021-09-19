using System;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using YukariToolBox.FormatLog;

namespace OneBot.CommandRoute.Utils
{
    public class YukariToolBoxLogger : ILogService
    {
        public ILogger<YukariToolBoxLogger> Logger { get; }

        public YukariToolBoxLogger(ILogger<YukariToolBoxLogger> logger)
        {
            Logger = logger;
        }

        public void Info(object type, object message)
        {
            Logger.LogInformation($"[{type}]{message}");
        }

        public void Warning(object type, object message)
        {
            Logger.LogWarning($"[{type}]{message}");

        }

        public void Error(object type, object message)
        {
            Logger.LogError($"[{type}]{message}");

        }

        public void Fatal(object type, object message)
        {
            Logger.LogCritical($"[{type}]{message}");
        }

        public void Debug(object type, object message)
        {
            Logger.LogDebug($"[{type}]{message}");
        }

        public void UnhandledExceptionLog(UnhandledExceptionEventArgs args)
        {
            StringBuilder errorLogBuilder = new StringBuilder();
            errorLogBuilder.Append("检测到未处理的异常");
            if (args.IsTerminating)
                errorLogBuilder.Append("，服务器将停止运行");
            Logger.LogError(0, args.ExceptionObject as Exception, errorLogBuilder.ToString());
            Warning("Sora", "将在5s后自动退出");
            Thread.Sleep(5000);
            Environment.Exit(-1);
        }
    }
}
