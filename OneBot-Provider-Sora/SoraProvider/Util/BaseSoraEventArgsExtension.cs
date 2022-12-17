﻿using System.Security.Cryptography;
using System.Text;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

namespace OneBot.Provider.SoraProvider;

public static class BaseSoraEventArgsExtension
{
    public static string GenerateId(this BaseSoraEventArgs e)
    {
        var s = $"{e.ServiceId}:{e.ConnId}:{e.EventName}:{e.LoginUid}:{e.Time.ToTimeStamp()}:{e.SourceType}";
        var bytes = MD5.HashData(Encoding.UTF8.GetBytes(s));

        StringBuilder sb = new StringBuilder();
        foreach (byte b in bytes)
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}