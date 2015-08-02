using MBotPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBot_2.Plugin
{
    public class BotUIHandler : IBotUI
    {
        public event OnUITabAddDelegate OnTabAdd;
        public LoadedPlugin Parent { get; private set; }
        public void AddTab(PluginTab tab)
        {
            if (OnTabAdd != null)
                OnTabAdd(Parent, tab);
        }
        public BotUIHandler(LoadedPlugin plugin)
        {
            Parent = plugin;
        }
    }
}
