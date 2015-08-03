using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBotPlugin
{
    public delegate void OnChatTrigger(string sender);
    public interface IPermissions
    {
        /// <summary>
        /// Allows direct access to twitch chat
        /// </summary>
        /// <returns></returns>
        Event.IChatTrigger UseChatTriggering();
        /// <summary>
        /// Allows use of twitch commands
        /// </summary>
        void UseCommands();
    }

    public static class Event
    {
        public interface IChatTrigger
        {
            void AddTrigger(string trigger, OnChatTrigger callback);
        }
    }
}
