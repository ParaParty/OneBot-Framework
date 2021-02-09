using System.Collections.Generic;
using Sora.Entities.CQCodes;
using Sora.Entities.CQCodes.CQCodeModel;
using Sora.Enumeration;

namespace QQRobot.CommandRoute
{
    /// <summary>
    /// 指令解析器
    /// </summary>
    public class CommandLaxer
    {
        /// <summary>
        /// 空白字符
        /// </summary>
        private const string BLANKCHARACTER = "\r\n\t ";

        /// <summary>
        /// 源信息
        /// </summary>
        public List<CQCode> SourceCommand { get; private set; }

        /// <summary>
        /// 已经解析了的参数信息
        /// </summary>
        public List<object> ParsedArguments { get; private set; } = new List<object>();

        /// <summary>
        /// 当前扫描到 <code>SourceCommand</code> 的哪一个位置。
        /// </summary>
        public int ScanObjectPointer { get; private set; } = 0;

        /// <summary>
        /// 如果当前扫描到的 <code>SourceCommand</code> 是 <code>CQFunction.Text</code> 的话当前字符串扫描到哪一个位置。
        /// </summary>
        public int ScanStringPointer { get; private set; } = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="s">被解析的字符串</param>
        public CommandLaxer(List<CQCode> s)
        {
            SourceCommand = s;
        }

        /// <summary>
        /// 检查被解析的字符串是否有效
        /// </summary>
        /// <returns>真: 有效/假: 无效</returns>
        public bool IsValid()
        {
            if (ScanObjectPointer != 0 || ScanStringPointer != 0) return true;
            if (SourceCommand.Count == 0) return false;
            if (SourceCommand[0].Function != CQFunction.Text) return false;
            var s = (Text)(SourceCommand[0].CQData);
            if (s.Content.Length == 0) return false;
            if (BLANKCHARACTER.Contains((s.Content[0]))) return false;

            return true;
        }

        /// <summary>
        /// 获取下一个 Token
        /// </summary>
        /// <returns>下一个 Token</returns>
        public object GetNext()
        {
            // 检查是否扫描到末尾
            if (ScanObjectPointer >= SourceCommand.Count)
            {
                throw new ParserToTheEndException();
            }
            var s = SourceCommand[ScanObjectPointer];

            // 跨越消息段
            if (s.Function != CQFunction.Text)
            {
                ScanObjectPointer++;
                ScanStringPointer = 0;
                ParsedArguments.Add(s);
                return s;
            }

            var str = ((Text)(s.CQData)).Content;
            var token = "";

            // 舍弃开头的空白字符
            while (ScanStringPointer < str.Length && BLANKCHARACTER.Contains(str[ScanStringPointer]))
            {
                ScanStringPointer++;
            }

            // 如果开头的空白字符删完之后没了那就表示下一个消息段必定不是文本
            if (ScanStringPointer >= str.Length)
            {
                ScanObjectPointer++;
                ScanStringPointer = 0;
                if (ScanObjectPointer >= SourceCommand.Count)
                {
                    throw new ParserToTheEndException();
                }
                var ret = SourceCommand[ScanObjectPointer];
                ScanObjectPointer++;
                ParsedArguments.Add(ret);
                return ret;
            }

            // 解析中间的字符
            // TODO 双引号
            while (ScanStringPointer < str.Length && (!BLANKCHARACTER.Contains(str[ScanStringPointer])))
            {
                token += str[ScanStringPointer];
                ScanStringPointer++;
            }

            if (ScanStringPointer >= str.Length)
            {
                ScanObjectPointer++;
                ScanStringPointer = 0;
            }

            ParsedArguments.Add(token);
            return token;
        }

        /// <summary>
        /// 获得现在已经得到的 Token 列表
        /// </summary>
        /// <returns>已获得的 Token 列表</returns>
        public List<object> GetNowParsedToken()
        {
            List<object> ret = new List<object>(ParsedArguments);

            if (ScanStringPointer != 0)
            {
                ret.Add(((Text)(SourceCommand[ScanObjectPointer].CQData)).Content.Substring(ScanStringPointer));
            }
            else
            {
                for (int i = ScanObjectPointer; i < SourceCommand.Count; i++)
                {
                    ret.Add(SourceCommand[i]);
                }
            }

            return ret;
        }

        /// <summary>
        /// 克隆 Laxer 的当前状态
        /// </summary>
        /// <returns>新的 Laxer</returns>
        public CommandLaxer Clone()
        {
            CommandLaxer ret = new CommandLaxer(SourceCommand);
            ret.ParsedArguments = new List<object>(ParsedArguments);
            ret.ScanObjectPointer = ScanObjectPointer;
            ret.ScanStringPointer = ScanStringPointer;
            return ret;
        }
    }
}