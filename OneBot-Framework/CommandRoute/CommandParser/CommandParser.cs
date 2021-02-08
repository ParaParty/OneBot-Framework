using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using QQRobot.Attribute;
using Sora.Entities.CQCodes;
using Sora.Entities.CQCodes.CQCodeModel;
using Sora.Enumeration;

namespace QQRobot.CommandRoute
{
    public class CommandParser
    {
        private const string BLANKCHARACTER = "\r\n\t ";

        public List<CQCode> SourceCommand { get; private set; }

        public List<object> ParsedArguments { get; private set; } = new List<object>();

        public int ScanObjectPointer { get; private set; } = 0;

        public int ScanStringPointer { get; private set; } = 0;

        public CommandParser(List<CQCode> s)
        {
            SourceCommand = s;
        }

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

        public object GetNext()
        {
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

        public List<object> GetNowParsedArguments()
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

        public CommandParser Clone()
        {
            CommandParser ret = new CommandParser(SourceCommand);
            ret.ParsedArguments = new List<object>(ParsedArguments);
            ret.ScanObjectPointer = ScanObjectPointer;
            ret.ScanStringPointer = ScanStringPointer;
            return ret;
        }
    }
}