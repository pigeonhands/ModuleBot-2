using MBotPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBot_2.Plugin
{
    public class BotHandler : IBot
    {
        public event MessageSayDelegate OnSayMessage;
        public event OnExceptionDelegate OnException;
        public LoadedPlugin Parent { get; private set; }
        public void SayMessage(string message, params object[] data)
        {
            try
            {
                if (OnSayMessage != null)
                    OnSayMessage(Parent, message, data);
            }
            catch(Exception ex)
            {
                if (OnException != null)
                    OnException(Parent, ex);
            }
        }

        public BotHandler(LoadedPlugin plugin)
        {
            Parent = plugin;
        }
    }
}
