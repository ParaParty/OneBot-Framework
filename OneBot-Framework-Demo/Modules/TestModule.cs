using System;
using Microsoft.Extensions.Logging;
using OneBot.CommandRoute.Attributes;
using OneBot.CommandRoute.Models.Enumeration;
using OneBot.CommandRoute.Services;
using OneBot.FrameworkDemo.Attributes;
using OneBot.FrameworkDemo.Models;
using Sora.Entities;

namespace OneBot.FrameworkDemo.Modules
{
    /// <summary>
    /// 这个是示例处理代码，所有的 OneBot 处理对象均需实现 IOneBotController 接口
    /// </summary>
    public class TestModule : IOneBotController
    {
        public TestModule(ICommandService commandService, ILogger<TestModule> logger)
        {
            // 通过构造函数获得指令路由服务对象
            // 绑定自己的处理事件
            commandService.Event.OnGroupMessage += (scope, args) =>
            {
                // 在控制台中复读群里的信息
                logger.LogInformation($"{args.SourceGroup.Id} : {args.Sender.Id} : {args.Message.RawText}");
                return 0;
            };

            commandService.Event.OnException += (scope, args, exception) =>
            {
                logger.LogError($"{exception.Message}");
            };
        }

        /// <summary>
        /// 定义一个指令：
        /// <para>使用 &lt; &gt; 括起来的是必选参数；</para>
        /// <para>使用 [ ] 括起来的是可选参数 </para>
        /// </summary>
        /// <param name="duration">禁言时长</param>
        /// <param name="args">全参数列表</param>
        /// <param name="userInfo">被禁言用户</param>
        [Command("mute <uid> [duration]", Alias = "禁言 <uid> [duration], 口球 <uid> [duration],", EventType = EventType.GroupMessage)]
        [DemoBeforeCommand]
        public void MuteInGroupWithDuration(Duration duration, [ParsedArguments] object[] args, [CommandParameter("uid")] User userInfo)
        {
            if (duration == null) duration = new Duration(600);
            Console.WriteLine($"禁言 {userInfo.Id} 用户 {duration.Seconds} 秒。");
        }

        /// <summary>
        /// 再定义一个指令：
        ///
        /// <para>参数不必要按顺序、参数会触发依赖注入</para>
        /// </summary>
        /// <param name="gid">操作群号</param>
        /// <param name="uid">被禁言用户</param>
        /// <param name="duration">禁言时长</param>
        [Command("mute <gid> <uid> [duration]", Alias = "禁言 <gid> <uid> [duration], 口球 <gid> <uid> [duration],", EventType = EventType.GroupMessage | EventType.PrivateMessage)]
        public void MuteInGroupWithGroupId(Group gid, User uid, Duration duration)
        {
            if (duration == null) duration = new Duration(600);
            Console.WriteLine($"禁言 {gid.Id} 群里的 {uid.Id} 用户 {duration.Seconds} 秒。");
        }  
        
        /// <summary>
        /// 全局异常处理函数测试
        /// </summary>
        [Command("exception", EventType = EventType.GroupMessage | EventType.PrivateMessage)]
        public void ExceptionTest()
        {
            throw new Exception("测试全局异常处理");
        }
    }

}
