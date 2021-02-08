using System;
using System.Text.RegularExpressions;

namespace QQRobot.Models.Model
{
    public class Duration
    {
        /// <summary>
        /// 时长
        /// </summary>
        public Int64 Seconds { get; private set; }
        public Duration(long seconds)
        {
            Seconds = seconds;
        }

        public static implicit operator Duration(Int32 seconds)
        {
            if (seconds < 0)
            {
                throw new ArgumentException($"无法解析参数。 {seconds}");
            }

            return new Duration(seconds);
        }

        public static implicit operator Duration(Int64 seconds)
        {
            if (seconds < 0)
            {
                throw new ArgumentException($"无法解析参数。 {seconds}");
            }

            return new Duration(seconds);
        }

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

        public static bool CanParse(string s)
        {
            return s != null && Regex.IsMatch(s, @"^(\d*d)?(\d*h)?(\d*m)?(\d*s)?$");
        }

        public static bool TryParse(string s, out Duration duration)
        {
            duration = null;
            if (s == null) return false;
            if (Int64.TryParse(s, out Int64 number))
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

    }
}