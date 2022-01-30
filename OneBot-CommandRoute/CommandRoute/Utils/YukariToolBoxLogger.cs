using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using YukariToolBox.LightLog;

namespace OneBot.CommandRoute.Utils;

public class YukariToolBoxLogger : ILogService
{
    private readonly ILoggerFactory _loggerFactory;

    private readonly ILogger<YukariToolBoxLogger> _logger;

    private readonly ConcurrentDictionary<Type, ILogger> _loggerMap = new ConcurrentDictionary<Type, ILogger>();

    public YukariToolBoxLogger(ILoggerFactory loggerFactory, ILogger<YukariToolBoxLogger> logger)
    {
        _loggerFactory = loggerFactory;
        _logger = logger;
    }

    private ILogger GetLogger()
    {
        StackTrace trace = new StackTrace();
        StackFrame? frame = trace.GetFrame(3);
        if (frame == null)
        {
            return _logger;
        }

        MethodBase? logMethod = frame.GetMethod();
        if (logMethod == null)
        {
            return _logger;
        }

        Type? logType = logMethod.ReflectedType;
        if (logType == null)
        {
            return _logger;
        }

        return _loggerMap.GetOrAdd(logType, s => _loggerFactory.CreateLogger(s));
    }


    public void Info(string source, string message)
    {
        GetLogger().LogInformation("[{source}] {message}", source, message);
    }

    public void Info<T>(string source, string message, T context)
    {
        GetLogger().LogInformation("[{source}] {message} {context}", source, message, context);
    }

    public void Warning(string source, string message)
    {
        GetLogger().LogWarning("[{source}] {message}", source, message);
    }

    public void Warning<T>(string source, string message, T context)
    {
        GetLogger().LogWarning("[{source}] {message} {context}", source, message, context);
    }

    public void Error(string source, string message)
    {
        GetLogger().LogError("[{source}] {message}", source, message);
    }

    public void Error(Exception exception, string source, string message)
    {
        GetLogger().LogError(exception, "[{source}] {message}", source, message);
    }

    public void Error<T>(string source, string message, T context)
    {
        GetLogger().LogError("[{source}] {message} {context}", source, message, context);
    }

    public void Error<T>(Exception exception, string source, string message, T context)
    {
        GetLogger().LogError(exception, "[{source}] {message} {context}", source, message, context);
    }

    public void Fatal(Exception exception, string source, string message)
    {
        GetLogger().LogCritical(exception, "[{source}] {message}", source, message);
    }

    public void Fatal<T>(Exception exception, string source, string message, T context)
    {
        GetLogger().LogCritical(exception, "[{source}] {message} {context}", source, message, context);
    }

    public void Debug(string source, string message)
    {
        GetLogger().LogDebug("[{source}] {message}", source, message);
    }

    public void Debug<T>(string source, string message, T context)
    {
        GetLogger().LogDebug("[{source}] {message} {context}", source, message, context);
    }

    public void Verbose(string source, string message)
    {
        GetLogger().LogDebug("[{source}] {message}", source, message);
    }

    public void Verbose<T>(string source, string message, T context)
    {
        GetLogger().LogDebug("[{source}] {message} {context}", source, message, context);
    }

    public void UnhandledExceptionLog(UnhandledExceptionEventArgs args)
    {
        StringBuilder errorLogBuilder = new StringBuilder();
        errorLogBuilder.Append("检测到未处理的异常");
        if (args.IsTerminating)
        {
            errorLogBuilder.Append("，服务器将停止运行");
        }

        _logger.LogCritical(args.ExceptionObject as Exception, "{errorLog}",errorLogBuilder.ToString());
        if (args.IsTerminating)
        {
            _logger.LogWarning("[Sora] 将在5s后自动退出");
            Thread.Sleep(5000);
            Environment.Exit(-1);
        }
    }

    public void SetCultureInfo(CultureInfo cultureInfo)
    {
        //
    }
}