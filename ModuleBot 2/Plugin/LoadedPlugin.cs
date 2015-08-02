using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MBotPlugin;

namespace ModuleBot_2.Plugin
{
    public class LoadedPlugin
    {
        public LoadedPlugin(Assembly asm, Type pluginType)
        {
            CanUseCommands = false;
            Enabled = false;
            PluginAssembly = asm;
            PluginInstance = (IPlugin)Activator.CreateInstance(pluginType);
            Details = PluginInstance.PluginDetails;
            PluginID = pluginType.GUID.ToString("n") + Details.Name;
        }

        public PluginInfomation Details { get; private set; }

        public void Initilze(HandlerBundle _handlers)
        {
            Handlers = _handlers;
            PluginInstance.PluginLoad(Handlers.Bot, Handlers.UI);
            Enabled = true;
        }

        public void LoopHandlers()
        {

        }

        public bool Enabled { get; set; }
        public Assembly PluginAssembly { get; private set; }
        public IPlugin PluginInstance { get; private set; }
        public HandlerBundle Handlers { get; private set; }
        public string PluginID { get; private set; }
        public bool CanUseCommands { get; set; }
    }
    
}
