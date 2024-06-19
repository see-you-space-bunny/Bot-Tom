using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace BotTom.Commands;

internal interface IUserDefinedCommand
{
    internal bool IsGlobal { get; }
    internal ulong Guild { get; }

    #pragma warning disable CS1998
    internal async Task RegisterCommand() { }
    internal async Task HandleSlashCommand(SocketSlashCommand command) { }
    internal async Task HandleCommand(Dictionary<string,object> commandInfo) { }
    #pragma warning restore CS1998
}