using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sora.Entities.CQCodes;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;
using Sora.Entities;
using OneBot.CommandRoute.Services;
using OneBot.CommandRoute.Attributes;
using OneBot.CommandRoute.Laxer;
using OneBot.CommandRoute.Models.Enumeration;
using OneBot_CommandRoute.CommandRoute.Attributes;

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
        public IOneBotController CommandObj { get; private set; }

        /// <summary>
        /// 指令方法
        /// </summary>
        public MethodInfo CommandMethod { get; private set; }

        /// <summary>
        /// 匹配的指令参数类型
        /// </summary>
        public List<Type> ParametersType { get; private set; }

        /// <summary>
        /// 指令匹配类型
        ///     0 全字匹配 /
        ///     1 参数 /
        ///     2 可选参数 /
        /// </summary>
        public List<int> ParametersMatchingType { get; private set; }

        /// <summary>
        /// 指令参数名
        /// </summary>
        public List<string> ParametersName { get; private set; }

        /// <summary>
        /// 匹配到的参数与方法参数的映射关系
        /// y = ParameterPositionMapping[x]
        /// x 是指令参数位置
        /// y 是函数参数位置
        /// </summary>
        public List<int> ParameterPositionMapping { get; private set; }

        /// <summary>
        /// 这个指令方法的属性
        /// </summary>
        public CommandAttribute Attribute { get; private set; }

        /// <summary>
        /// 全字匹配和必选参数的个数和
        /// </summary>
        public int WeightA { get; private set; }

        /// <summary>
        /// 可选参数的个数
        /// </summary>
        public int WeightB { get; private set; }

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
        /// <param name="scope">事件上下文</param>
        /// <param name="sender">事件触发者</param>
        /// <param name="baseSoraEventArgs">Sora 事件对象</param>
        /// <param name="laxer">指令解析器</param>
        /// <returns>0 继续 / 1 阻断</returns>
        public int Invoke(IServiceScope scope, object sender, BaseSoraEventArgs baseSoraEventArgs, CommandLaxer laxer)
        {
            // 检查事件类型是否正确
            if (baseSoraEventArgs is PrivateMessageEventArgs &&
                (Attribute.EventType & EventType.PrivateMessage) == 0)
            {
                return 0;
            }

            if (baseSoraEventArgs is GroupMessageEventArgs &&
                (Attribute.EventType & EventType.GroupMessage) == 0)
            {
                return 0;
            }


            // 尝试解析剩下的所有参数
            int step = laxer.ParsedArguments.Count;

            List<object> matchedArgs = new List<object>();
            matchedArgs.AddRange(laxer.ParsedArguments);
            for (int i = step; i < ParametersName.Count; i++)
            {
                matchedArgs.Add(null);
            }

            bool needExecute = true;
            for (int i = step; i < ParametersName.Count; i++)
            {
                var newParser = laxer.Clone();
                // 解析一个新的

                object newArg = null;
                bool succeed = false;

                // 解析新的参数
                try
                {
                    newArg = newParser.GetNext();
                    succeed = true;
                }
                catch (Exception)
                {
                    succeed = false;
                }

                // 类型转换
                if (succeed) { 
                    if (ParametersMatchingType[i] == 0)
                    {
                        succeed = (newArg is string) && ((string)newArg) == ParametersName[i];
                    }
                    else
                    {
                        succeed = TryParseType(baseSoraEventArgs, newArg, ParametersType[i], out object result);
                        if (succeed)
                        { 
                            newArg = result;
                        }
                    }
                }

                if (succeed)
                {
                    matchedArgs[i] = newArg;
                    laxer = newParser;
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
            object[] functionArgs = new object[functionParametersList.Length];
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
                var ParameterType = parameter.ParameterType;

                // 判断是否需要传递所有的参数
                if (System.Attribute.IsDefined(parameter, typeof(ParsedArgumentsAttribute)))
                {
                    if (ParameterType == typeof(object[]))
                    {
                        functionArgs[i] = laxer.GetNowParsedToken().ToArray();
                    }
                    else
                    {
                        Console.Error.WriteLine("[ParsedArguments] 属性仅接受 object[] 类型。");
                    }
                    continue;
                }
                
                // 判断是否需要传递事件信息
                if (ParameterType == typeof(BaseSoraEventArgs))
                {
                    functionArgs[i] = baseSoraEventArgs;
                    continue;
                }

                // 判断是否需要传递 Scope 信息
                if (ParameterType == typeof(IServiceScope))
                {
                    functionArgs[i] = scope;
                    continue;
                }

                // 从 Scope 中获得参数
                functionArgs[i] = scope.ServiceProvider.GetService(ParameterType);
            }

            // TODO 判断是否有基本类型但是是 NOTNULL 的。

            // 在调用前执行
            if (System.Attribute.IsDefined(CommandMethod, typeof(BeforeCommandAttribute)))
            {
                var attrs = System.Attribute.GetCustomAttributes(CommandMethod, typeof(BeforeCommandAttribute));
                for (int i = 0; i < attrs.Length; i++)
                {
                    (attrs[i] as BeforeCommandAttribute)?.Invoke(scope, baseSoraEventArgs);
                }
            }

            // 调用
            if (CommandMethod.ReturnType == typeof(int))
            {
                return (int) CommandMethod.Invoke(CommandObj, functionArgs);
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
        private bool TryParseType(BaseSoraEventArgs baseSoraEventArgs, object arg, Type type, out object result)
        {
            result = null;

            if (arg is string)
            {
                var s = arg as string;
                try
                {
                    return TryParseString(baseSoraEventArgs, s, type, out result);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            if (arg is CQCode)
            {
                var s = arg as CQCode;
                try
                {
                    return TryParseCQCode(baseSoraEventArgs, s, type, out result);
                }
                catch (Exception)
                {
                    return false;
                }
                
            }

            return false;
        }

        /// <summary>
        /// 尝试解析 CQ 码
        /// </summary>
        /// <param name="baseSoraEventArgs">Sora 事件对象</param>
        /// <param name="arg">被 cast 的值</param>
        /// <param name="type">要 cast 的类型</param>
        /// <param name="result">cast 结果</param>
        /// <returns>真: 成功 / 假: 失败</returns>
        private bool TryParseCQCode(BaseSoraEventArgs baseSoraEventArgs, CQCode arg, Type type, out object result)
        {
            bool ret = false;
            if (type == arg.CQData.GetType())
            {
                ret = true;
                result = arg;
            }
            else if (arg.Function == CQFunction.At)
            {
                var cast = (Sora.Entities.CQCodes.CQCodeModel.At)arg.CQData;
                var succeed = long.TryParse(cast.Traget, out long uid);
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

        /// <summary>
        /// 尝试解析字符串
        /// </summary>
        /// <param name="baseSoraEventArgs">Sora 事件对象</param>
        /// <param name="arg">被 cast 的值</param>
        /// <param name="type">要 cast 的类型</param>
        /// <param name="result">cast 结果</param>
        /// <returns>真: 成功 / 假: 失败</returns>
        private bool TryParseString(BaseSoraEventArgs baseSoraEventArgs, string arg, Type type, out object result)
        {
            bool ret = false;

            if (type == typeof(int))
            {
                ret = int.TryParse(arg, out int number);
                result = number;
            }
            else if (type == typeof(long))
            {
                ret = long.TryParse(arg, out long number);
                result = number;
            }
            else if (type == typeof(double))
            {
                ret = double.TryParse(arg, out double number);
                result = number;
            }
            else if (type == typeof(decimal))
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
            else if(type == typeof(User))
            {
                ret = long.TryParse(arg, out long cast);
                result = baseSoraEventArgs.SoraApi.GetUser(cast);
            }
            else if(type == typeof(Group))
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
                        result = converter.Invoke(null, new[] { arg });
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