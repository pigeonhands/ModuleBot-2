using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBotPlugin
{
    public delegate void CommandCallback(string sender, string[] Parameters, IUserData data);
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
        public UserInput[] UserData { get { return _UserData.ToArray() ; } }
        private List<UserInput> _UserData = new List<UserInput>();
        private HashSet<string> UserInputID = new HashSet<string>();

        public void Execute(string sender, string[] Parameters, IUserData userData)
        {
            OnExecute(sender, Parameters, userData);
        }

        public void AddUserInput(params UserInput[] inp)
        {
            foreach (UserInput u in inp)
            {
                if (u == null)
                    continue;
                if(UserInputID.Add(u.ID))
                    _UserData.Add(u);
            }
                
        }

        public static PluginCommand Create(string name, string descrption, CommandCallback cb, ParamiterType Parameters, params UserInput[] inp)
        {
            PluginCommand pt = new PluginCommand(name);
            pt.Description = descrption;
            pt.OnExecute += cb;
            pt.Paramiter = Parameters;
            pt.AddUserInput(inp);
            return pt;
        }
    }
    
    public enum ParamiterType
    {
        None,
        Optional,
        Must
    }
}
