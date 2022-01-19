using Microsoft.Extensions.DependencyInjection;

using OneBot.CommandRoute.Attributes;
using OneBot.CommandRoute.Lexer;
using OneBot.CommandRoute.Models.Enumeration;
using OneBot.CommandRoute.Services;

using Sora.Entities;
using Sora.Entities.Segment;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;
using Sora.Util;

using System.Reflection;

namespace OneBot.CommandRoute.Models.Entities
{
    /// <summary>
    /// 指令信息
    /// </summary>
    public class CommandModel
    {
        /// <summary>
        /// 指令对象（单例）
        /// </summary>
        public IOneBotController CommandObj { get; }

        /// <summary>
        /// 指令方法
        /// </summary>
        public MethodInfo CommandMethod { get; }

        /// <summary>
        /// 匹配的指令参数类型
        /// </summary>
        public List<Type> ParametersType { get; }

        /// <summary>
        /// 指令匹配类型
        ///     0 全字匹配 /
        ///     1 参数 /
        ///     2 可选参数 /
        /// </summary>
        public List<int> ParametersMatchingType { get; }

        /// <summary>
        /// 指令参数名
        /// </summary>
        public List<string> ParametersName { get; }

        /// <summary>
        /// 匹配到的参数与方法参数的映射关系
        /// y = ParameterPositionMapping[x]
        /// x 是指令参数位置
        /// y 是函数参数位置
        /// </summary>
        public List<int> ParameterPositionMapping { get; }

        /// <summary>
        /// 这个指令方法的属性
        /// </summary>
        public CommandAttribute Attribute { get; }

        /// <summary>
        /// 全字匹配和必选参数的个数和
        /// </summary>
        public int WeightA { get; }

        /// <summary>
        /// 可选参数的个数
        /// </summary>
        public int WeightB { get; }

        /// <summary>
        /// 构造函数
        ///
        /// 需保证匹配类型中可选参数必须全部在最后
        /// </summary>
        /// <param name="commandObj">指令对象</param>
        /// <param name="commandMethod">指令方法</param>
        /// <param name="parametersType">指令参数类型</param>
        /// <param name="parametersMatchingType">指令匹配类型</param>
        /// <param name="parametersName">指令参数名</param>
        /// <param name="parameterPositionMapping">指令参数位置到方法参数位置的映射关系</param>
        /// <param name="attribute">指令方法的属性</param>
        public CommandModel(IOneBotController commandObj, MethodInfo commandMethod, List<Type> parametersType,
            List<int> parametersMatchingType, List<string> parametersName, List<int> parameterPositionMapping,
            CommandAttribute attribute)
        {
            CommandObj = commandObj;
            CommandMethod = commandMethod;
            ParametersType = parametersType;
            ParametersMatchingType = parametersMatchingType;
            ParametersName = parametersName;
            ParameterPositionMapping = parameterPositionMapping;
            Attribute = attribute;

            WeightA = parametersMatchingType.Count(s => s != 2);
            WeightB = parametersMatchingType.Count(s => s == 2);
        }

        /// <summary>
        /// 尝试调用这个函数
        /// </summary>
        /// <param name="context">事件上下文</param>
        /// <param name="lexer">指令解析器</param>
        /// <returns>0 继续 / 1 阻断</returns>
        public int Invoke(OneBotContext context, CommandLexer lexer)
        {
            var baseSoraEventArgs = context.SoraEventArgs;
            switch (baseSoraEventArgs)
            {
                // 检查事件类型是否正确
                case PrivateMessageEventArgs when (Attribute.EventType & EventType.PrivateMessage) == 0:
                case GroupMessageEventArgs when (Attribute.EventType & EventType.GroupMessage) == 0:
                    return 0;
            }

            // 尝试解析剩下的所有参数
            int step = lexer.ParsedArguments.Count;

            List<object?> matchedArgs = new();
            matchedArgs.AddRange(lexer.ParsedArguments);
            for (int i = step; i < ParametersName.Count; i++)
            {
                matchedArgs.Add(null);
            }

            bool needExecute = true;
            for (int i = step; i < ParametersName.Count; i++)
            {
                var newParser = lexer.Clone();
                // 解析一个新的

                object? newArg = null;
                // ReSharper disable once RedundantAssignment
                bool succeed;

                // 解析新的参数
                try
                {
                    newArg = newParser.GetNextNotBlank();
                    succeed = true;
                }
                catch (Exception)
                {
                    succeed = false;
                }

                // 类型转换
                if (succeed)
                {
                    if (ParametersMatchingType[i] == 0)
                    {
                        succeed = newArg is string name && name == ParametersName[i];
                    }
                    else
                    {
                        succeed = TryParseType(baseSoraEventArgs, newArg, ParametersType[i], out object? result);
                        if (succeed)
                        {
                            newArg = result;
                        }
                    }
                }

                if (succeed)
                {
                    matchedArgs[i] = newArg;
                    lexer = newParser;
                }
                else
                {
                    // 如果解析失败就判断是否需要继续执行
                    // 如果当前已经匹配到了可选参数那就可以继续执行
                    needExecute = ParametersMatchingType[i] == 2;
                    break;
                }
            }

            if (!needExecute) return 0;

            // 调用
            // 参数注入
            var functionParametersList = CommandMethod.GetParameters();
            object?[] functionArgs = new object[functionParametersList.Length];
            for (int i = 0; i < ParametersName.Count; i++)
            {
                if (ParameterPositionMapping[i] < 0) continue;
                functionArgs[ParameterPositionMapping[i]] = matchedArgs[i];
            }

            // 依赖注入
            for (int i = 0; i < functionParametersList.Length; i++)
            {
                if (functionArgs[i] != null) continue;

                var parameter = functionParametersList[i];
                var parameterType = parameter.ParameterType;

                // 判断是否需要传递所有的参数
                if (System.Attribute.IsDefined(parameter, typeof(ParsedArgumentsAttribute)))
                {
                    if (parameterType == typeof(object[]))
                    {
                        functionArgs[i] = lexer.GetNowParsedToken().ToArray();
                    }
                    else
                    {
                        throw new ArgumentException($"[ParsedArguments] 属性仅接受 object[] 类型。");
                    }

                    continue;
                }

                // 判断是否需要传递事件信息
                if (parameterType == typeof(BaseSoraEventArgs) ||
                    parameterType == typeof(PrivateMessageEventArgs) && baseSoraEventArgs is PrivateMessageEventArgs ||
                    parameterType == typeof(GroupMessageEventArgs) && baseSoraEventArgs is GroupMessageEventArgs)
                {
                    functionArgs[i] = baseSoraEventArgs;
                    continue;
                }

                // 判断是否需要传递 Scope 信息
                if (parameterType == typeof(IServiceScope))
                {
                    functionArgs[i] = context.ServiceScope;
                    continue;
                }

                if (parameterType == typeof(OneBotContext))
                {
                    functionArgs[i] = context;
                    continue;
                }

                // 从 Scope 中获得参数
                functionArgs[i] = context.ServiceScope.ServiceProvider.GetService(parameterType);
            }

            // TODO 判断是否有基本类型但是是 NOTNULL 的。

            // 在调用前执行
            if (System.Attribute.IsDefined(CommandMethod, typeof(BeforeCommandAttribute)))
            {
                var attrs = System.Attribute.GetCustomAttributes(CommandMethod, typeof(BeforeCommandAttribute));
                for (int i = 0; i < attrs.Length; i++)
                {
                    (attrs[i] as BeforeCommandAttribute)?.Invoke(context);
                }
            }

            // 调用
            if (CommandMethod.ReturnType == typeof(int))
            {
                return CommandMethod.Invoke(CommandObj, functionArgs) is int i ? i : throw new InvalidCastException();
            }

            CommandMethod.Invoke(CommandObj, functionArgs);
            return 1;
        }

        /// <summary>
        /// 尝试解析一个参数
        /// </summary>
        /// <param name="baseSoraEventArgs">Sora 事件对象</param>
        /// <param name="arg">被 cast 的值</param>
        /// <param name="type">要 cast 的类型</param>
        /// <param name="result">cast 结果</param>
        /// <returns>真: 成功 / 假: 失败</returns>
        private static bool TryParseType(BaseSoraEventArgs baseSoraEventArgs, object? arg, Type type, out object? result)
        {
            result = null;

            switch (arg)
            {
                case string s:
                    try
                    {
                        return TryParseString(baseSoraEventArgs, s, type, out result);
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                case SoraSegment s:
                    try
                    {
                        return TryParseCQCode(baseSoraEventArgs, s, type, out result);
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                case MessageBody s:
                    try
                    {
                        return TryParseMessageBody(baseSoraEventArgs, s, type, out result);
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        /// <summary>
        /// 尝试解析 CQ 码
        /// </summary>
        /// <param name="baseSoraEventArgs">Sora 事件对象</param>
        /// <param name="arg">被 cast 的值</param>
        /// <param name="type">要 cast 的类型</param>
        /// <param name="result">cast 结果</param>
        /// <returns>真: 成功 / 假: 失败</returns>
        private static bool TryParseMessageBody(BaseSoraEventArgs baseSoraEventArgs, MessageBody arg, Type type, out object? result)
        {
            // ReSharper disable once RedundantAssignment
            bool ret;
            if (type == typeof(MessageBody))
            {
                ret = true;
                result = arg;
            }
            else if (type == typeof(string))
            {
                ret = true;
                result = arg.Serialize();
            }
            else
            {
                try
                {
                    var converter = type.GetMethod("op_Implicit", new[] { arg.GetType() });
                    if (converter != null)
                    {
                        result = converter.Invoke(null, new[] { arg });
                        // ReSharper disable once RedundantAssignment
                        ret = true;
                    }
                    else
                    {
                        result = null;
                        // ReSharper disable once RedundantAssignment
                        ret = false;
                    }
                }
                catch (Exception)
                {
                    // ReSharper disable once RedundantAssignment
                }
                result = null;
                return false;
            }

            return ret;
        }

        /// <summary>
        /// 尝试解析 CQ 码
        /// </summary>
        /// <param name="baseSoraEventArgs">Sora 事件对象</param>
        /// <param name="arg">被 cast 的值</param>
        /// <param name="type">要 cast 的类型</param>
        /// <param name="result">cast 结果</param>
        /// <returns>真: 成功 / 假: 失败</returns>
        private static bool TryParseCQCode(BaseSoraEventArgs baseSoraEventArgs, object arg, Type type, out object? result)
        {
            // ReSharper disable once RedundantAssignment
            bool ret = false;
            if (type == ((SoraSegment)arg).Data.GetType())
            {
                ret = true;
                result = arg;
            }
            else if (((SoraSegment)arg).MessageType == SegmentType.At)
            {
                var cast = (Sora.Entities.Segment.DataModel.AtSegment)((SoraSegment)arg).Data;
                var succeed = long.TryParse(cast.Target, out long uid);
                if (!succeed)
                {
                    result = null;
                }
                else if (type == typeof(long))
                {
                    result = uid;
                    ret = true;
                }
                else if (type == typeof(User))
                {
                    result = baseSoraEventArgs.SoraApi.GetUser(uid);
                    ret = true;
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                try
                {
                    var converter = type.GetMethod("op_Implicit", new[] { arg.GetType() });
                    if (converter != null)
                    {
                        result = converter.Invoke(null, new[] { arg });
                        // ReSharper disable once RedundantAssignment
                        ret = true;
                    }
                    else
                    {
                        result = null;
                        // ReSharper disable once RedundantAssignment
                        ret = false;
                    }
                }
                catch (Exception)
                {
                    // ReSharper disable once RedundantAssignment
                }
                result = null;
                return false;
            }

            return ret;
        }

        /// <summary>
        /// 尝试解析字符串
        /// </summary>
        /// <param name="baseSoraEventArgs">Sora 事件对象</param>
        /// <param name="arg">被 cast 的值</param>
        /// <param name="type">要 cast 的类型</param>
        /// <param name="result">cast 结果</param>
        /// <returns>真: 成功 / 假: 失败</returns>
        private static bool TryParseString(BaseSoraEventArgs baseSoraEventArgs, string? arg, Type type, out object? result)
        {
            // ReSharper disable once RedundantAssignment
            bool ret;
            if (type == typeof(int) || type == typeof(int?))
            {
                ret = int.TryParse(arg, out int number);
                result = number;
            }
            else if (type == typeof(long) || type == typeof(long?))
            {
                ret = long.TryParse(arg, out long number);
                result = number;
            }
            else if (type == typeof(double) || type == typeof(double?))
            {
                ret = double.TryParse(arg, out double number);
                result = number;
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                ret = decimal.TryParse(arg, out decimal number);
                result = number;
            }
            else if (type == typeof(string))
            {
                result = arg;
                ret = true;
            }
            /*else if (type == typeof(Duration))
            {
                ret = Duration.TryParse(arg, out Duration duration);
                result = duration;
            }*/
            else if (type == typeof(User))
            {
                ret = long.TryParse(arg, out long cast);
                result = baseSoraEventArgs.SoraApi.GetUser(cast);
            }
            else if (type == typeof(Group))
            {
                ret = long.TryParse(arg, out long cast);
                result = baseSoraEventArgs.SoraApi.GetGroup(cast);
            }
            else
            {
                try
                {
                    var converter = type.GetMethod("op_Implicit", new[] { typeof(string) });
                    if (converter != null)
                    {
                        result = converter.Invoke(null, new object?[] { arg });
                        ret = true;
                    }
                    else
                    {
                        result = null;
                        ret = false;
                    }
                }
                catch (Exception)
                {
                    result = null;
                    ret = false;
                }
            }

            return ret;
        }
    }
}
