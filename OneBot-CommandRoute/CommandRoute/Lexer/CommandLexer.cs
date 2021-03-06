using System.Collections.Generic;
using Sora.Entities.MessageElement.CQModel;
using Sora.Entities.MessageElement;
using Sora.Enumeration;

namespace OneBot.CommandRoute.Lexer
{
    /// <summary>
    /// 指令解析器
    /// </summary>
    public class CommandLexer
    {
        /// <summary>
        /// 空白字符
        /// </summary>
        private const string BLANKCHARACTER = "\r\n\t ";

        /// <summary>
        /// 源信息
        /// </summary>
        public IList<CQCode> SourceCommand { get; private set; }

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
        public CommandLexer(IList<CQCode> s)
        {
            SourceCommand = s;
        }

        /// <summary>
        /// 检查被解析的字符串是否有效
        /// </summary>
        /// <returns>真: 有效/假: 无效</returns>
        public bool IsValid()
        {
            // 如果已经开始扫描了那必然合法
            if (ScanObjectPointer != 0 || ScanStringPointer != 0) return true;

            var scanObjectPointer = 0;
            var scanStringPointer = 0;

            // 空消息不合法
            if (SourceCommand.Count == 0) return false;

            // 如果第一个消息段为回复消息
            if (SourceCommand[0].MessageType == CQType.Reply)
            {
                // 如果消息段长度只有1则不合法
                if (SourceCommand.Count < 2) return false;
                scanObjectPointer = 1;
            }


            string s;
            var flag = false;

            // 兼容 回复+At+(空格+正文) 的奇怪设计
            if (scanObjectPointer == 1 && SourceCommand[scanObjectPointer].MessageType == CQType.At)
            {
                if (SourceCommand[scanObjectPointer].MessageType == CQType.At)
                {
                    // 如果消息段长度只有2则不合法
                    if (SourceCommand.Count < 3) return false;
                    scanObjectPointer = 2;
                }

                flag = true;
            }

            // 如果扫描起始消息段不为文本则不合法
            if (SourceCommand[scanObjectPointer].MessageType != CQType.Text) return false;
            s = ((Text) SourceCommand[scanObjectPointer].DataObject).Content;

            // 兼容 回复+空格+At+正文 的奇怪设计
            if (scanObjectPointer == 1 && string.IsNullOrWhiteSpace(s))
            {
                if (SourceCommand.Count < 4) return false;
                if (SourceCommand[2].MessageType != CQType.At) return false;
                if (SourceCommand[3].MessageType != CQType.Text) return false;

                scanObjectPointer = 3;
                s = ((Text) SourceCommand[scanObjectPointer].DataObject).Content;

                flag = true;
            }

            if (flag)
            {
                // 舍弃空白段
                while (SourceCommand[scanObjectPointer].MessageType == CQType.Text)
                {
                    if (!string.IsNullOrWhiteSpace(((Text) SourceCommand[scanObjectPointer].DataObject).Content)) break;
                    scanObjectPointer++;
                    if (scanObjectPointer == SourceCommand.Count) return false;
                }

                // 如果舍弃空白段后不是文本段则不合法
                if (SourceCommand[scanObjectPointer].MessageType != CQType.Text) return false;
                s = ((Text) (SourceCommand[scanObjectPointer].DataObject)).Content;

                // 舍弃开头空白字符
                while (scanStringPointer < s.Length && BLANKCHARACTER.Contains(s[scanStringPointer]))
                {
                    scanStringPointer++;
                }
            }

            // 普通发言支持
            if (scanObjectPointer == 0)
            {
                // 空消息不合法
                if (s.Length == 0) return false;

                // 空白符号开头不合法
                if (BLANKCHARACTER.Contains((s[0]))) return false;
            }

            ScanObjectPointer = scanObjectPointer;
            ScanStringPointer = scanStringPointer;
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

            // 当前扫描到的消息段
            var s = SourceCommand[ScanObjectPointer];

            // 如果当前扫描到的消息段不是文本消息段则返回当前消息段
            if (s.MessageType != CQType.Text)
            {
                ScanObjectPointer++;
                ScanStringPointer = 0;
                ParsedArguments.Add(s);
                return s;
            }

            // 获取当前消息段的文本
            var str = ((Text) (s.DataObject)).Content;
            var token = "";

            while (ScanStringPointer < str.Length && BLANKCHARACTER.Contains(str[ScanStringPointer]))
            {
                // 舍弃开头的空白字符
                while (ScanStringPointer < str.Length && BLANKCHARACTER.Contains(str[ScanStringPointer]))
                {
                    ScanStringPointer++;
                }

                // 如果开头的空白字符删完之后没了就要从下一个消息段继续扫描
                if (ScanStringPointer >= str.Length)
                {
                    ScanObjectPointer++;
                    ScanStringPointer = 0;
                    if (ScanObjectPointer >= SourceCommand.Count)
                    {
                        throw new ParserToTheEndException();
                    }

                    // 获取下一个消息段
                    var ret = SourceCommand[ScanObjectPointer];

                    // 如果下一个消息段不是文本则返回
                    if (ret.MessageType != CQType.Text)
                    {
                        ScanObjectPointer++;
                        ParsedArguments.Add(ret);
                        return ret;
                    }

                    // 如果下一个消息段是文本则继续扫描
                    s = ret;
                    str = ((Text) (s.DataObject)).Content;
                }
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
                ret.Add(((Text) (SourceCommand[ScanObjectPointer].DataObject)).Content.Substring(ScanStringPointer));
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
        /// 克隆 Lexer 的当前状态
        /// </summary>
        /// <returns>新的 Lexer</returns>
        public CommandLexer Clone()
        {
            CommandLexer ret = new CommandLexer(SourceCommand);
            ret.ParsedArguments = new List<object>(ParsedArguments);
            ret.ScanObjectPointer = ScanObjectPointer;
            ret.ScanStringPointer = ScanStringPointer;
            return ret;
        }
    }
}