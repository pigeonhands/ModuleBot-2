using MBotPlugin;
using ModuleBot_2.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBot_2.Commands
{
    public class CommandHandler
    {
        public LoadedPlugin Parent { get; private set; }
        public PluginCommand Command { get; private set; }
        public string ID { get; private set; }

        public CommandHandler(LoadedPlugin plugin, PluginCommand _command)
        {
            Parent = plugin;
            Command = _command;
            ID = string.Format("{0}->[{1}]", plugin.PluginID, _command.Name);
        }
    }
}
