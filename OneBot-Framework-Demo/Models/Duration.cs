using System;
using System.Text.RegularExpressions;

namespace OneBot.FrameworkDemo.Models
{
    /// <summary>
    /// 时长类型；
    /// 这里是示例自定义参数类型；
    /// 需实现一个从 String 或 CQCode 类型来的隐式转换函数才能触发类型解析。
    /// </summary>
    public class Duration
    {
        /// <summary>
        /// 时长
        /// </summary>
        public long Seconds { get; private set; }
        public Duration(long seconds)
        {
            Seconds = seconds;
        }

        /// <summary>
        /// 实现一个从数字到 Duration 的隐式转换
        /// </summary>
        /// <param name="seconds">时长</param>
        public static implicit operator Duration(int seconds)
        {
            if (seconds < 0)
            {
                throw new ArgumentException($"无法解析参数。 {seconds}");
            }

            return new Duration(seconds);
        }

        /// <summary>
        /// 实现一个从数字到 Duration 的隐式转换
        /// </summary>
        /// <param name="seconds">时长</param>
        public static implicit operator Duration(long seconds)
        {
            if (seconds < 0)
            {
                throw new ArgumentException($"无法解析参数。 {seconds}");
            }

            return new Duration(seconds);
        }

        /// <summary>
        /// 实现一个从字符串到 Duration 的隐式转换。
        /// 1d2h3m4s -> 1天2小时3分钟4秒
        /// </summary>
        /// <param name="value">时长</param>
        public static implicit operator Duration(string value)
        {
            if (!CanParse(value))
            {
                throw new ArgumentException($"无法解析参数。 {value}");
            }

            var s = "";
            long ret = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if ((value[i] == 's') || (value[i] == 'S'))
                {
                    ret += Convert.ToInt64(s);
                    s = "";
                    continue;
                }

                if ((value[i] == 'm') || (value[i] == 'M'))
                {
                    ret += Convert.ToInt64(s) * 60;
                    s = "";
                    continue;
                }

                if ((value[i] == 'h') || (value[i] == 'H'))
                {
                    ret += Convert.ToInt64(s) * 3600;
                    s = "";
                    continue;
                }

                if ((value[i] == 'd') || (value[i] == 'D'))
                {
                    ret += Convert.ToInt64(s) * 86400;
                    s = "";
                    continue;
                }

                s += value[i];
            }
            return new Duration(ret);
        }

        /// <summary>
        /// 判断能否解析
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool CanParse(string s)
        {
            return s != null && Regex.IsMatch(s, @"^(\d*d)?(\d*h)?(\d*m)?(\d*s)?$");
        }

        /// <summary>
        /// 尝试解析
        /// </summary>
        /// <param name="s"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out Duration duration)
        {
            duration = null;
            if (s == null) return false;
            if (long.TryParse(s, out long number))
            {
                duration = number;
            }
            else if (Regex.IsMatch(s, @"^(\d*d)?(\d*h)?(\d*m)?(\d*s)?$"))
            {
                duration = s;
            }
            else return false;
            return true;
        }

        /// <summary>
        /// 返回可用 ^(\d*d)?(\d*h)?(\d*m)?(\d*s)?$ 匹配的最简时长表达式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var temp = Seconds;
            var ret = "";
            var day = temp / 86400;
            if (day > 0) ret += $"{day}d";
            temp %= 86400;
            var hour = temp / 3600;
            if (hour > 0) ret += $"{hour}h";
            temp %= 3600;
            var min = temp / 60;
            if (min > 0) ret += $"{min}m";
            temp %= 60;
            if (temp > 0) ret += $"{temp}s";
            return ret;
        }
    }
}