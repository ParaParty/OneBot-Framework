using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using QQRobot.Attribute;
using QQRobot.Services;
using Sora.Entities.CQCodes;
using Sora.Enumeration;
using Sora.EventArgs.SoraEvent;
using Sora.Entities;

namespace QQRobot.CommandRoute
{
    public class CommandModel
    {
        public IQQRobotService CommandObj { get; private set; }
        public MethodInfo CommandMethod { get; private set; }
        public List<Type> ParametersType { get; private set; }
        public List<int> ParametersMatchingType { get; private set; }
        public List<string> ParametersName { get; private set; }
        public List<int> ParameterPositionMapping { get; private set; }
        public CommandAttribute Attribute { get; private set; }

        public int WeightA { get; private set; }
        public int WeightB { get; private set; }

        public CommandModel(IQQRobotService commandObj, MethodInfo commandMethod, List<Type> parametersType,
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

        public int Invoke(IServiceScope scope, object sender, BaseSoraEventArgs baseSoraEventArgs, CommandParser parser)
        {
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
            int step = parser.ParsedArguments.Count;

            List<object> matchedArgs = new List<object>();
            matchedArgs.AddRange(parser.ParsedArguments);
            for (int i = step; i < ParametersName.Count; i++)
            {
                matchedArgs.Add(null);
            }

            bool needExecute = true;
            for (int i = step; i < ParametersName.Count; i++)
            {
                var newParser = parser.Clone();
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
                        succeed = (newArg is String) && ((String)newArg) == ParametersName[i];
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
                    parser = newParser;
                }
                else
                {
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
                        functionArgs[i] = parser.GetNowParsedArguments().ToArray();
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

            if (CommandMethod.ReturnType == typeof(int))
            {
                return (int) CommandMethod.Invoke(CommandObj, functionArgs);
            }

            CommandMethod.Invoke(CommandObj, functionArgs);
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseSoraEventArgs">消息事件对象，这个你不用管</param>
        /// <param name="arg">被 cast 的值</param>
        /// <param name="type">要 cast 的类型</param>
        /// <param name="result">cast 结果</param>
        /// <returns></returns>
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
                    result = converter.Invoke(null, new[] { arg });
                    ret = true;
                }
                catch (Exception)
                {
                    result = null;
                    ret = false;
                }
            }

            return ret;
        }

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
                ret = Double.TryParse(arg, out double number);
                result = number;
            }
            else if (type == typeof(decimal))
            {
                ret = Decimal.TryParse(arg, out decimal number);
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
                    result = converter.Invoke(null, new[] { arg });
                    ret = true;
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