using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBotPlugin
{
    public delegate void OnRawChat(string sender, string message);
    public interface IPluginEvent
    {
        string Description { get; }
    }
    public static class PluginEvent
    {
        public class RawChatEvent : IPluginEvent
        {
            public string Description
            {
                get { return "Access to raw chat"; }
            }

            public event OnRawChat OnMessage;

            public void Execute(string sender, string message)
            {
                try
                {
                    OnMessage(sender, message);
                }
                catch
                {

                }
            }
        }
        public class UseChatCommands : IPluginEvent
        {
            public string Description
            {
                get
                {
                    return "Ability to use twitch commands";
                }
            }
        }
    }
}
