using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBotPlugin;
using static MBotPlugin.Event;

namespace ModuleBot_2.Plugin.Permissions.Handlers
{
    public class ChatTriggerHandler : IChatTrigger
    {
        public OnExceptionDelegate OnException { get; set; }
        private Dictionary<string, OnChatTrigger> TriggerList = new Dictionary<string, OnChatTrigger>();
        public void CheckTrigger(LoadedPlugin plugin, string sender, string message)
        {
            try
            {
                foreach(var trigger in TriggerList)
                {
                    if (message.ToLower().Contains(trigger.Key))
                        trigger.Value(sender);
                }
                
            }
            catch(Exception ex)
            {
                if(OnException!=null)
                    OnException(plugin, ex);
            }
        }

        public void AddTrigger(string trigger, OnChatTrigger callback)
        {
            if (TriggerList.ContainsKey(trigger.ToLower()))
                TriggerList[trigger.ToLower()] += callback;
            else
                TriggerList.Add(trigger.ToLower(), callback);
        }
    }
}
