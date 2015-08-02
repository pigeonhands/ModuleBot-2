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
        public IPluginEvent[] EventList
        {
            get { return _EventList.ToArray(); }
        }
        private List<PluginCommand> _LoadedCommands = new List<PluginCommand>();
        private List<IPluginEvent> _EventList = new List<IPluginEvent>();
        public PluginInfomation(string _name)
        {
            Name = _name;
            if (string.IsNullOrEmpty(Name))
                Name = "Unnamed";
        }
        public void AddCommand(PluginCommand command)
        {
            _LoadedCommands.Add(command);
        }

        public void AddCommands(params PluginCommand[] commands)
        {
            foreach (var c in commands)
                AddCommand(c);
        }
        public void AddEvent(IPluginEvent pluginevent)
        {
            foreach(IPluginEvent e in EventList)
            {
                if (e.GetType() == pluginevent.GetType())
                    return;
            }
            _EventList.Add(pluginevent);
        }
        public string Name { get; private set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
    }
}
