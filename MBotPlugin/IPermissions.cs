using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBotPlugin
{
    public delegate void OnRawChat(string sender, string message);
    public interface IPermissions
    {
        /// <summary>
        /// Allows direct access to twitch chat
        /// </summary>
        /// <returns></returns>
        Event.IRawChatHandler AccessRawChat();
        /// <summary>
        /// Allows use of twitch commands
        /// </summary>
        void AccessCommands();
    }

    public static class Event
    {
        public interface IRawChatHandler
        {
            event OnRawChat OnChat;
        }
    }
}
