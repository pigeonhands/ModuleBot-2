using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBotPlugin
{
    public delegate void CommandCallback(string sender, string[] paramiters);
    public class PluginCommand
    {
        public PluginCommand(string _name)
        {
            Name = _name;
            Paramiter = ParamiterType.None;
        }
        public event CommandCallback OnExecute;
        public string Name { get; private set; }
        public string Description { get; set; }
        public ParamiterType Paramiter { get; set; }

        public void Execute(string sender, string[] paramiters)
        {
            try
            {
                OnExecute(sender, paramiters);
            }
            catch
            {

            }
        }
    }
    
    public enum ParamiterType
    {
        None,
        Optional,
        Must
    }
}
