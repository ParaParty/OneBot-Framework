﻿namespace OneBot.CommandRoute.Model.Notice;

public interface GroupMemberIncrease
{
    string DetailType { get; }
    string SubType { get; }
    string GroupId { get; }
    string UserId { get; }
    string Operator { get; }
}
