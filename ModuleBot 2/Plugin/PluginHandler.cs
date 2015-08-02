using MBotPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBot_2.Plugin
{
    public delegate void MessageSayDelegate(LoadedPlugin parent, string message, params object[] data);
    public delegate void OnExceptionDelegate(LoadedPlugin parent, Exception ex);
    public delegate void OnUITabAddDelegate(LoadedPlugin parent, PluginTab tab);
    public static class PluginHandler
    {
        public static LoadedPlugin LoadPlugin(string path)
        {
            Assembly asm = Assembly.LoadFile(path);
            foreach (Type t in asm.GetTypes())
            {
                if (typeof(IPlugin).IsAssignableFrom(t))
                {
                    return new LoadedPlugin(asm, t);
                }
            }
            return null;
        }
    }

    public class HandlerBundle
    {
        public BotHandler Bot { get; set; }
        public BotUIHandler UI { get; set; }
    }
}
