using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OneBot.CommandRoute.Attributes;
using OneBot.CommandRoute.Services;
using OneBot_CommandRoute.CommandRoute.Attributes;
using Sora.EventArgs.SoraEvent;

namespace OneBot.CommandRoute.Models.Entities
{
    public class CQJsonRouteModel
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
        /// 这个路由方法的属性
        /// </summary>
        public CQJsonAttribute Attribute { get; private set; }
        
        
        public CQJsonRouteModel(IOneBotController commandObj, MethodInfo commandMethod, CQJsonAttribute attribute)
        {
            CommandObj = commandObj;
            CommandMethod = commandMethod;
            Attribute = attribute;
        }

        public int Invoke(IServiceScope scope, BaseSoraEventArgs baseSoraEventArgs)
        {
            var functionParametersList = CommandMethod.GetParameters();
            object[] functionArgs = new object[functionParametersList.Length];
            
            // 依赖注入
            for (int i = 0; i < functionParametersList.Length; i++)
            {
                if (functionArgs[i] != null) continue;

                var parameter = functionParametersList[i];
                var parameterType = parameter.ParameterType;

                // 判断是否需要传递事件信息
                if (parameterType == typeof(BaseSoraEventArgs))
                {
                    functionArgs[i] = baseSoraEventArgs;
                    continue;
                }

                if (parameterType == typeof(PrivateMessageEventArgs) && baseSoraEventArgs is PrivateMessageEventArgs)
                {
                    functionArgs[i] = baseSoraEventArgs;
                    continue;
                }

                if (parameterType == typeof(GroupMessageEventArgs) && baseSoraEventArgs is GroupMessageEventArgs)
                {
                    functionArgs[i] = baseSoraEventArgs;
                    continue;
                }

                // 判断是否需要传递 Scope 信息
                if (parameterType == typeof(IServiceScope))
                {
                    functionArgs[i] = scope;
                    continue;
                }

                // 从 Scope 中获得参数
                functionArgs[i] = scope.ServiceProvider.GetService(parameterType);
            }
            
            // 在调用前执行
            if (System.Attribute.IsDefined(CommandMethod, typeof(BeforeCommandAttribute)))
            {
                var attrs = System.Attribute.GetCustomAttributes(CommandMethod, typeof(BeforeCommandAttribute));
                if (attrs.Select(t => (t as BeforeCommandAttribute)?.Invoke(scope, baseSoraEventArgs)).Any(beforeReturn => beforeReturn.HasValue && !beforeReturn.Value))
                {
                    //拦截一波，返回false 则不进行指令执行，拦截掉
                    return 1;
                }
            }

            // 调用
            if (CommandMethod.ReturnType == typeof(int))
            {
                // ReSharper disable once PossibleNullReferenceException
                return (int) CommandMethod.Invoke(CommandObj, functionArgs);
            }

            CommandMethod.Invoke(CommandObj, functionArgs);
            return 1;
        }
    }
}