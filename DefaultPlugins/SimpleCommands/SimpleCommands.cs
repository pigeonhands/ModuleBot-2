using MBotPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCommands
{
    public class SimpleCommands : IPlugin
    {

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
                     PluginCommand.Create("Ban", "Ban a user. Parameters: <username>", BanCommand_OnExecute, ParamiterType.Must),
                     PluginCommand.Create("Echo", "Echo what the user types (Filters commands)", EchoCommand_OnExecute, ParamiterType.Must),
                     PluginCommand.Create("Timeout", "Timeout a user. Parameters: !timeout <username> <optional time>", TimeoutCommand_OnExecute, ParamiterType.Must),
                     PluginCommand.Create("Say", "Say Pre-Defined text", SayCommand_OnExecute, ParamiterType.None, UserInput.Create("Text", typeof(string)))
                     );

                return info;
            }
        }
        private void BanCommand_OnExecute(string sender, string[] Parameters, IUserData data)
        {
            Bot.SayMessage("/ban {0}", Parameters[0]);
        }

        private void TimeoutCommand_OnExecute(string sender, string[] Parameters, IUserData data)
        {
            if(Parameters.Length < 3)
                Bot.SayMessage("/timeout {0}", string.Join(" ", (Parameters)));
        }
        private void SayCommand_OnExecute(string sender, string[] Parameters, IUserData data)
        {
            Bot.SayMessage("{0}", data.GetValue<object>("Text", ""));
        }

        private void ClearChatCommand_OnExecute(string sender, string[] Parameters, IUserData data)
        {
            Bot.SayMessage("/clear");
        }

        private void EchoCommand_OnExecute(string sender, string[] Parameters, IUserData data)
        {
            if (Parameters[0].StartsWith("/"))
            {
                if (Parameters.Length == 1)
                    Parameters[0] = string.Empty;
                if (Parameters[0].Length > 1)
                    Parameters[0] = Parameters[0].Substring(1, Parameters[0].Length - 1);
            }
            Bot.SayMessage(string.Join(" ", Parameters));
        }

        public void PluginLoad(IBot BotHandler, IBotUI UIHandler, IPermissions PermissionsHandler)
        {
            Bot = BotHandler;
            PermissionsHandler.UseCommands();
        }
    }
}
