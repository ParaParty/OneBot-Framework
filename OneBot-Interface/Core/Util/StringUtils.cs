using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OneBot.Core.Util;

internal static class StringUtils
{
    public const char Underline = '_';

    public enum CamelCaseType
    {
        Upper, Lower
    }
    public enum SnakeCaseType
    {
        Lower, Upper
    }


    public static bool IsNullOrEmpty(string? value)
        => string.IsNullOrEmpty(value);

    public static bool IsNullOrWhiteSpace(string? value)
        => string.IsNullOrWhiteSpace(value);

    public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
    {
        if (format == null)
            throw new ArgumentNullException(nameof(format));
        return string.Format(provider, format, args);
    }

    /// <summary>
    /// 获取字符数组中包含指定字符的个数
    /// </summary>
    /// <param name="s"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static int GetCharCount(char[] s, char c)
    {
        int cnt = 0;
        for (int i = 0; i < s.Length; i++)
            if (s[i] == c)
                cnt++;
        return cnt;
    }

    /// <summary>
    /// 蛇形转驼峰
    /// </summary>
    /// <param name="s">蛇形字符串</param>
    /// <param name="caseType">驼峰类型</param>
    /// <param name="lockedWords">锁定的单词(例如：QQ，此类整个词为单位需要统一大小写的词，<b>大写传入</b>)</param>
    /// <returns></returns>
    public static string ToCamelCase(string s, CamelCaseType caseType, string[]? lockedWords = null)
    {
        if (IsNullOrEmpty(s))
            return s;

        char[] src = s.ToCharArray();
        int ulCnt = GetCharCount(src, Underline);

        char[] result = new char[src.Length - ulCnt];
        bool fstWord = true, fstLetter = true;
        string tmp = "";
        for (int i = 0, j = 0;i <= src.Length; i++)
        {
            if (i == src.Length || src[i] == Underline || char.IsSeparator(src[i]))
            {
                if (lockedWords != null && lockedWords.Contains(tmp))
                {
                    bool isUpper = char.IsUpper(result[j - tmp.Length]);
                    for (int k = 1; k < tmp.Length; k++)
                    {
                        if (isUpper)
                            result[j - tmp.Length + k] = char.ToUpper(result[j - tmp.Length + k]);
                        else
                            result[j - tmp.Length + k] = char.ToLower(result[j - tmp.Length + k]);
                    }
                }
                if (i < src.Length)
                {
                    if (src[i] == Underline)
                    {
                        fstLetter = true;
                        tmp = "";
                    }
                    else
                    {
                        fstWord = true;
                        fstLetter = true;
                        result[j] = src[i];
                        tmp = "";
                        j++;
                    }
                }
            }
            else
            {
                if (fstLetter)
                {
                    if (caseType == CamelCaseType.Lower && fstWord)
                        result[j] = char.ToLower(src[i]);
                    else
                        result[j] = char.ToUpper(src[i]);
                    if (fstWord)
                        fstWord = false;
                    fstLetter = false;
                }
                else
                {
                    result[j] = char.ToLower(src[i]);
                }
                tmp += char.ToUpper(src[i]);
                j++;
            }
        }

        return new string(result);
    }

    /// <summary>
    /// 驼峰转蛇形
    /// </summary>
    /// <param name="s">驼峰字符串</param>
    /// <param name="caseType">蛇形类型</param>
    /// <param name="lockedWords">锁定的单词(例如：QQ，此类整个词为单位需要统一大小写的词，<b>大写传入</b>)</param>
    /// <returns></returns>
    public static string ToSnakeCase(string s, SnakeCaseType caseType, string[]? lockedWords = null)
    {
        StringBuilder sb = new StringBuilder();
        char[] src = s.ToCharArray();

        bool fstWord = true;
        for(int i = 0; i < src.Length; i++)
        {
            if (char.IsSeparator(src[i]))
            {
                sb.Append(src[i]);
                fstWord = true;
                continue;
            }
            if (char.IsUpper(src[i]))
            {
                if (lockedWords != null)
                {
                    bool skip = false;
                    for (int j = 0; j < lockedWords.Length; j++)
                    {
                        bool match = true;
                        for (int k = 0; k < lockedWords[j].Length; k++)
                        {
                            if (i + k >= src.Length || lockedWords[j][k] != char.ToUpper(src[i + k]))
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            if (!fstWord)
                                sb.Append("_");
                            else
                                fstWord = false;
                            for (int k = 0; k < lockedWords[j].Length; k++)
                            {
                                if (caseType == SnakeCaseType.Upper)
                                    sb.Append(char.ToUpper(src[i]));
                                else
                                    sb.Append(char.ToLower(src[i]));
                                i++;
                            }
                            i--;
                            skip = true;
                            break;
                        }
                    }
                    if (skip)
                        continue;
                }
                if (!fstWord)
                    sb.Append("_");
                else
                    fstWord = false;
            }
            if (caseType == SnakeCaseType.Upper)
                sb.Append(char.ToUpper(src[i]));
            else
                sb.Append(char.ToLower(src[i]));
        }

        return sb.ToString();
    }
}

