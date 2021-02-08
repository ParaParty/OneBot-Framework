using System;
using QQRobot.Attribute;
using QQRobot.CommandRoute;
using QQRobot.Models.Model;
using QQRobot.Services;
using Sora.Entities;

namespace QQRobot.QQRobot
{
    public class TestModule : IQQRobotService
    {
        public TestModule(ICommandService commandService)
        {
            commandService.Event.OnGroupMessage += (scope, args) =>
            {
                // 在控制台中复读群里的信息
                Console.WriteLine($"{args.SourceGroup.Id} : {args.Sender.Id} : {args.Message.RawText}");
                return 0;
            };
        }

        [Command("mute <uid> [duration]", Alias = "禁言 <uid> [duration], 口球 <uid> [duration],", EventType = EventType.GroupMessage)]
        public void MuteInGroupWithDuration(Duration duration, [ParsedArguments] Object[] args, [CommandParameter("uid")] User uid)
        {
            if (duration == null) duration = new Duration(600);
            Console.WriteLine($"禁言 {uid.Id} 用户 {duration.Seconds} 秒。");
        }

        [Command("mute <gid> <uid> [duration]", Alias = "禁言 <gid> <uid> [duration], 口球 <gid> <uid> [duration],", EventType = EventType.GroupMessage)]
        public void MuteInGroupWithGroupId(Group gid, User uid, Duration duration)
        {
            if (duration == null) duration = new Duration(600);
            Console.WriteLine($"禁言 {gid.Id} 群里的 {uid.Id} 用户 {duration.Seconds} 秒。");
        }
    }

}
