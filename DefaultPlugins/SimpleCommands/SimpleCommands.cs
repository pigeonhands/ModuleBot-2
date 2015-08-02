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

                PluginCommand echoCommand = new PluginCommand("Echo");
                echoCommand.Description = "Echo what the user types (Filters commands)";
                echoCommand.Paramiter = ParamiterType.Must;
                echoCommand.OnExecute += EchoCommand_OnExecute;

                PluginCommand clearChatCommand = new PluginCommand("Clear chat");
                clearChatCommand.Description = "Clears the chat";
                clearChatCommand.OnExecute += ClearChatCommand_OnExecute;

                PluginCommand banCommand = new PluginCommand("Ban");
                banCommand.Description = "Ban a user";
                banCommand.Paramiter = ParamiterType.Must;
                banCommand.OnExecute += BanCommand_OnExecute; ;

                info.AddCommands(echoCommand, clearChatCommand, banCommand);


                PluginEvent.RawChatEvent chatEvent = new PluginEvent.RawChatEvent();
                chatEvent.OnMessage += (sender, message) =>
                {
                    chatLog.NewMessage(sender, message);
                };
                info.AddEvent(chatEvent);
                info.AddEvent(new PluginEvent.UseChatCommands());

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

        public void PluginLoad(IBot BotHandler, IBotUI UIHandler)
        {
            Bot = BotHandler;
            UIHandler.AddTab(new PluginTab("Raw chat", chatLog));
        }
    }
}
