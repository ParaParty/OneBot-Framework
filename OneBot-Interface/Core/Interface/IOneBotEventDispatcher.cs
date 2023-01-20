﻿using System.Threading.Tasks;
using OneBot.Core.Event;
using OneBot.Core.Model;

namespace OneBot.Core.Interface;

public interface IOneBotEventDispatcher
{
    ValueTask Fire(OneBotEvent e);
}
