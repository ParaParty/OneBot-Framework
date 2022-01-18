using System;
using Microsoft.Extensions.DependencyInjection;
using OneBot.CommandRoute.Attributes;
using OneBot.CommandRoute.Models;
using Sora.Entities.Info;
using Sora.Enumeration.ApiType;
using Sora.Enumeration.EventParamsType;
using Sora.EventArgs.SoraEvent;

namespace OneBot.FrameworkDemo.Attributes
{
    /// <summary>
    /// 在触发指令前的切片
    /// </summary>
    public class DemoBeforeCommandAttribute : BeforeCommandAttribute
    {
        public override void Invoke(OneBotContext context)
        {
            IServiceScope scope = context.ServiceScope;
            BaseSoraEventArgs baseSoraEventArgs = context.SoraEventArgs;
            
            GroupMessageEventArgs p = baseSoraEventArgs as GroupMessageEventArgs;
            if (p == null) return;

            var taskValue = p.SoraApi.GetGroupMemberInfo(p.SourceGroup, p.Sender.Id, true);
            taskValue.AsTask().Wait();

            if (taskValue.Result.apiStatus.RetCode != ApiStatusType.OK)
            {
                return;
            }

            GroupMemberInfo uInfo = taskValue.Result.memberInfo;

            if (uInfo.Role != MemberRoleType.Member)
            {
                Console.WriteLine($"发送者 {uInfo.UserId} 是管理员");
            }
            else
            {
                Console.WriteLine($"发送者 {uInfo.UserId} 不是管理员");
            }
        }
    }
}
