using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBotPlugin
{
    public class PluginInfomation
    {
        public PluginCommand[] LoadedCommands
        {
            get { return _LoadedCommands.ToArray(); }
        }
        private List<PluginCommand> _LoadedCommands = new List<PluginCommand>();
        private HashSet<string> PluginNames = new HashSet<string>();
        public PluginInfomation(string _name)
        {
            Name = _name;
            if (string.IsNullOrEmpty(Name))
                Name = "Unnamed";
        }
        public void AddCommand(PluginCommand command)
        {
            if(PluginNames.Add(command.Name))
            {
                _LoadedCommands.Add(command);
            }
        }

        public void AddCommands(params PluginCommand[] commands)
        {
            foreach (var c in commands)
                AddCommand(c);
        }
        public string Name { get; private set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
    }
}
