using MBotPlugin;
using SimpleCommands.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCommands
{
    public class SimpleCommands : IPlugin
    {
        RawChatLog chatLog = new RawChatLog();

        IBot Bot;
        public PluginInfomation PluginDetails
        {
            get
            {
                PluginInfomation info = new PluginInfomation("SimpleCommands");
                info.Description = "Simple commands for ModuleBot";
                info.Publisher = "BahNahNah";

                info.AddCommands(
                     PluginCommand.Create("Clear chat", "Clears the chat", ClearChatCommand_OnExecute, ParamiterType.None),
                     PluginCommand.Create("Ban", "Ban a user", BanCommand_OnExecute, ParamiterType.Must),
                     PluginCommand.Create("Echo", "Echo what the user types (Filters commands)", EchoCommand_OnExecute, ParamiterType.Must)
                     );

                return info;
            }
        }

        private void BanCommand_OnExecute(string sender, string[] paramiters)
        {
            Bot.SayMessage("/ban {0}", paramiters[0]);
        }

        private void ClearChatCommand_OnExecute(string sender, string[] paramiters)
        {
            Bot.SayMessage("/clear");
        }

        private void EchoCommand_OnExecute(string sender, string[] paramiters)
        {
            if (paramiters[0].StartsWith("/"))
            {
                if (paramiters.Length == 1)
                    paramiters[0] = string.Empty;
                if (paramiters[0].Length > 1)
                    paramiters[0] = paramiters[0].Substring(1, paramiters[0].Length - 1);
            }
            Bot.SayMessage(string.Join(" ", paramiters));
        }

        public void PluginLoad(IBot BotHandler, IBotUI UIHandler, IPermissions PermissionsHandler)
        {
            Bot = BotHandler;

            Event.IRawChatHandler rawChatHandler = PermissionsHandler.AccessRawChat();
            rawChatHandler.OnChat += RawChatHandler_OnChat;

            UIHandler.AddTab(new PluginTab("Raw chat", chatLog));
        }

        private void RawChatHandler_OnChat(string sender, string message)
        {
            chatLog.NewMessage(sender, message);
        }
    }
}
